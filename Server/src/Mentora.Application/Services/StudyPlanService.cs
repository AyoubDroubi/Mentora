using Mentora.Application.DTOs.Assessment;
using Mentora.Application.Interfaces.Repositories;
using Mentora.Application.Interfaces.Services;
using Mentora.Application.Services.AI;
using Mentora.Domain.Entities;
using Mentora.Domain.Entities.Assessment;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Mentora.Application.Services
{
    /// <summary>
    /// Service interface for study plan generation and management per SRS Section 3 & 4
    /// </summary>
    public interface IStudyPlanService
    {
        Task<GenerateStudyPlanResponseDto> GenerateStudyPlanAsync(Guid userId, GenerateStudyPlanRequestDto request);
        Task<StudyPlanDto> GetStudyPlanAsync(Guid userId, Guid studyPlanId, bool includeAll = true);
        Task<List<StudyPlanDto>> GetUserStudyPlansAsync(Guid userId);
        Task<StudyPlanDto?> GetActiveStudyPlanAsync(Guid userId);
        Task UpdateProgressAsync(Guid userId, UpdateStudyPlanProgressDto update);
        Task<SkillGapAnalysisDto> AnalyzeSkillGapsAsync(Guid userId, Guid studyPlanId);
        Task ActivateStudyPlanAsync(Guid userId, Guid studyPlanId);
        Task ArchiveStudyPlanAsync(Guid userId, Guid studyPlanId);
    }

    /// <summary>
    /// Implementation of study plan service per SRS 3.2
    /// Orchestrates AI generation, relational parsing, and skill gap analysis
    /// </summary>
    public class StudyPlanService : IStudyPlanService
    {
        private readonly IAssessmentRepository _assessmentRepo;
        private readonly ISkillRepository _skillRepo;
        private readonly IUserProfileRepository _profileRepo;
        private readonly ICareerPlanRepository _careerPlanRepo;
        private readonly IAiStudyPlanService _aiService;
        private readonly AssessmentContextBuilder _contextBuilder;
        private readonly ILogger<StudyPlanService> _logger;

        public StudyPlanService(
            IAssessmentRepository assessmentRepo,
            ISkillRepository skillRepo,
            IUserProfileRepository profileRepo,
            ICareerPlanRepository careerPlanRepo,
            IAiStudyPlanService aiService,
            AssessmentContextBuilder contextBuilder,
            ILogger<StudyPlanService> logger)
        {
            _assessmentRepo = assessmentRepo;
            _skillRepo = skillRepo;
            _profileRepo = profileRepo;
            _careerPlanRepo = careerPlanRepo;
            _aiService = aiService;
            _contextBuilder = contextBuilder;
            _logger = logger;
        }

        /// <summary>
        /// Generate AI study plan per SRS 3.2
        /// </summary>
        public async Task<GenerateStudyPlanResponseDto> GenerateStudyPlanAsync(
            Guid userId,
            GenerateStudyPlanRequestDto request)
        {
            _logger.LogInformation($"?? Generating study plan for user {userId}");

            // 1. Get assessment context per SRS 3.1.2
            var attempt = await _assessmentRepo.GetAttemptByIdAsync(request.AssessmentAttemptId, includeResponses: true);
            if (attempt == null || attempt.UserId != userId)
                throw new UnauthorizedAccessException("Invalid assessment attempt");

            if (attempt.Status != Domain.Entities.Assessment.AssessmentStatus.Completed)
                throw new InvalidOperationException("Assessment must be completed before generating study plan");

            var context = _contextBuilder.DeserializeContext(attempt.ContextJson!)
                ?? throw new InvalidOperationException("Assessment context not found");

            // 2. Enhance context with additional data
            await EnhanceContextAsync(context, userId, request);

            // 3. Call AI service per SRS 3.2.1
            _logger.LogInformation("?? Calling AI service for study plan generation...");
            var aiResponse = await _aiService.GenerateStudyPlanAsync(context, request.AdditionalInstructions);

            // 4. Parse into relational entities per SRS 3.2.2
            _logger.LogInformation("?? Parsing AI response into database entities...");
            var studyPlan = await ParseAndPersistStudyPlanAsync(userId, attempt, request, aiResponse);

            // 5. Perform skill gap analysis per SRS 3.3
            _logger.LogInformation("?? Performing skill gap analysis...");
            await PerformSkillGapAnalysisAsync(userId, studyPlan);

            _logger.LogInformation($"? Study plan {studyPlan.Id} generated successfully!");

            return new GenerateStudyPlanResponseDto
            {
                StudyPlanId = studyPlan.Id,
                Title = studyPlan.Title,
                Status = studyPlan.Status.ToString(),
                TotalSteps = studyPlan.Steps.Count,
                TotalResources = studyPlan.Resources.Count,
                EstimatedHours = studyPlan.EstimatedHours,
                RequiredSkills = studyPlan.RequiredSkills.Select(s => s.Skill.Name).ToList(),
                GeneratedAt = studyPlan.CreatedAt
            };
        }

        /// <summary>
        /// Get study plan with details per SRS 3.2
        /// </summary>
        public async Task<StudyPlanDto> GetStudyPlanAsync(Guid userId, Guid studyPlanId, bool includeAll = true)
        {
            var studyPlan = await _assessmentRepo.GetStudyPlanByIdAsync(studyPlanId, includeAll);
            if (studyPlan == null || studyPlan.UserId != userId)
                throw new UnauthorizedAccessException("Study plan not found or access denied");

            return MapToDto(studyPlan);
        }

        /// <summary>
        /// Get all study plans for user
        /// </summary>
        public async Task<List<StudyPlanDto>> GetUserStudyPlansAsync(Guid userId)
        {
            var plans = await _assessmentRepo.GetUserStudyPlansAsync(userId);
            return plans.Select(MapToDto).ToList();
        }

        /// <summary>
        /// Get active study plan
        /// </summary>
        public async Task<StudyPlanDto?> GetActiveStudyPlanAsync(Guid userId)
        {
            var plan = await _assessmentRepo.GetActiveStudyPlanAsync(userId);
            return plan != null ? MapToDto(plan) : null;
        }

        /// <summary>
        /// Update study plan progress per SRS 5.2
        /// </summary>
        public async Task UpdateProgressAsync(Guid userId, UpdateStudyPlanProgressDto update)
        {
            var studyPlan = await _assessmentRepo.GetStudyPlanByIdAsync(update.StudyPlanId, includeAll: true);
            if (studyPlan == null || studyPlan.UserId != userId)
                throw new UnauthorizedAccessException("Study plan not found");

            // Update checkpoint
            if (update.CheckpointId.HasValue && update.IsCompleted.HasValue)
            {
                await _assessmentRepo.UpdateCheckpointCompletionAsync(update.CheckpointId.Value, update.IsCompleted.Value);
            }

            // Update step progress
            if (update.StepId.HasValue && update.ProgressPercentage.HasValue)
            {
                await _assessmentRepo.UpdateStepProgressAsync(update.StepId.Value, update.ProgressPercentage.Value);
            }

            // Recalculate overall progress per SRS 5.2.1
            await RecalculateOverallProgressAsync(studyPlan);

            _logger.LogInformation($"? Progress updated for study plan {update.StudyPlanId}");
        }

        /// <summary>
        /// Analyze skill gaps per SRS 3.3.3
        /// </summary>
        public async Task<SkillGapAnalysisDto> AnalyzeSkillGapsAsync(Guid userId, Guid studyPlanId)
        {
            var studyPlan = await _assessmentRepo.GetStudyPlanByIdAsync(studyPlanId, includeAll: true);
            if (studyPlan == null || studyPlan.UserId != userId)
                throw new UnauthorizedAccessException("Study plan not found");

            var userProfile = await _profileRepo.GetByUserIdAsync(userId);
            if (userProfile == null)
                throw new InvalidOperationException("User profile not found");

            var userSkills = userProfile.Skills.ToList();
            var gaps = new List<SkillGapDto>();

            foreach (var requiredSkill in studyPlan.RequiredSkills)
            {
                var userSkill = userSkills.FirstOrDefault(us => us.SkillId == requiredSkill.SkillId);
                
                var gap = new SkillGapDto
                {
                    SkillName = requiredSkill.Skill.Name,
                    TargetLevel = requiredSkill.TargetProficiency.ToString(),
                    CurrentLevel = userSkill?.ProficiencyLevel.ToString() ?? "None",
                    Gap = requiredSkill.ProficiencyGap,
                    Status = requiredSkill.Status.ToString(),
                    Priority = requiredSkill.Importance <= 2 ? "Low" : requiredSkill.Importance == 3 ? "Medium" : "High"
                };

                // Find relevant resources
                gap.RecommendedResources = studyPlan.Resources
                    .Where(r => r.Title.Contains(requiredSkill.Skill.Name, StringComparison.OrdinalIgnoreCase))
                    .Take(3)
                    .Select(r => r.Title)
                    .ToList();

                gaps.Add(gap);
            }

            return new SkillGapAnalysisDto
            {
                StudyPlanId = studyPlanId,
                TotalRequiredSkills = studyPlan.RequiredSkills.Count,
                MissingSkills = gaps.Count(g => g.CurrentLevel == "None"),
                InProgressSkills = gaps.Count(g => g.Status == "InProgress"),
                AchievedSkills = gaps.Count(g => g.Status == "Achieved"),
                Gaps = gaps,
                Recommendations = GenerateRecommendations(gaps)
            };
        }

        /// <summary>
        /// Activate study plan per SRS 3.2
        /// </summary>
        public async Task ActivateStudyPlanAsync(Guid userId, Guid studyPlanId)
        {
            // Deactivate other plans
            var userPlans = await _assessmentRepo.GetUserStudyPlansAsync(userId);
            foreach (var plan in userPlans.Where(p => p.IsActive && p.Id != studyPlanId))
            {
                plan.IsActive = false;
                await _assessmentRepo.UpdateStudyPlanAsync(plan);
            }

            // Activate target plan
            var targetPlan = await _assessmentRepo.GetStudyPlanByIdAsync(studyPlanId);
            if (targetPlan == null || targetPlan.UserId != userId)
                throw new UnauthorizedAccessException("Study plan not found");

            targetPlan.IsActive = true;
            targetPlan.Status = Domain.Entities.Assessment.StudyPlanStatus.Active;
            targetPlan.StartedAt ??= DateTime.UtcNow;
            await _assessmentRepo.UpdateStudyPlanAsync(targetPlan);

            _logger.LogInformation($"? Study plan {studyPlanId} activated");
        }

        /// <summary>
        /// Archive study plan
        /// </summary>
        public async Task ArchiveStudyPlanAsync(Guid userId, Guid studyPlanId)
        {
            var studyPlan = await _assessmentRepo.GetStudyPlanByIdAsync(studyPlanId);
            if (studyPlan == null || studyPlan.UserId != userId)
                throw new UnauthorizedAccessException("Study plan not found");

            studyPlan.Status = Domain.Entities.Assessment.StudyPlanStatus.Archived;
            studyPlan.IsActive = false;
            await _assessmentRepo.UpdateStudyPlanAsync(studyPlan);

            _logger.LogInformation($"?? Study plan {studyPlanId} archived");
        }

        // Private helper methods

        private async Task EnhanceContextAsync(AssessmentContextDto context, Guid userId, GenerateStudyPlanRequestDto request)
        {
            // Add preferred completion date
            if (request.PreferredCompletionDate.HasValue)
            {
                var weeksUntil = (request.PreferredCompletionDate.Value - DateTime.UtcNow).TotalDays / 7;
                context.AdditionalContext["PreferredWeeks"] = weeksUntil.ToString("F0");
            }

            // Add weekly hours if specified
            if (request.WeeklyHoursAvailable.HasValue)
            {
                context.WeeklyHoursAvailable = request.WeeklyHoursAvailable.Value;
            }

            // Add focus areas
            if (request.FocusAreas != null && request.FocusAreas.Any())
            {
                context.InterestedAreas.AddRange(request.FocusAreas);
            }

            // Add career plan context if linked
            if (request.CareerPlanId.HasValue)
            {
                var careerPlan = await _careerPlanRepo.GetByIdWithStepsAsync(request.CareerPlanId.Value);
                if (careerPlan != null)
                {
                    context.CareerGoal = careerPlan.Title;
                    context.AdditionalContext["CareerPlanTitle"] = careerPlan.Title;
                }
            }
        }

        private async Task<AiStudyPlan> ParseAndPersistStudyPlanAsync(
            Guid userId,
            AssessmentAttempt attempt,
            GenerateStudyPlanRequestDto request,
            AiStudyPlanResponse aiResponse)
        {
            // Create container per SRS 3.2.2
            var studyPlan = new AiStudyPlan
            {
                UserId = userId,
                AssessmentAttemptId = attempt.Id,
                CareerPlanId = request.CareerPlanId,
                Title = aiResponse.Title,
                Summary = aiResponse.Summary,
                EstimatedHours = aiResponse.EstimatedHours,
                DifficultyLevel = Enum.Parse<Domain.Entities.Assessment.DifficultyLevel>(aiResponse.DifficultyLevel),
                Status = Domain.Entities.Assessment.StudyPlanStatus.Draft,
                TargetCompletionDate = request.PreferredCompletionDate,
                AiModel = aiResponse.AiModel,
                RawAiResponseJson = aiResponse.RawResponse
            };

            // Parse steps per SRS 3.2.2
            foreach (var aiStep in aiResponse.Steps.OrderBy(s => s.OrderIndex))
            {
                var step = new StudyPlanStep
                {
                    Name = aiStep.Name,
                    Description = aiStep.Description,
                    OrderIndex = aiStep.OrderIndex,
                    EstimatedHours = aiStep.EstimatedHours,
                    Status = aiStep.OrderIndex == 0 ? StepStatus.NotStarted : StepStatus.Locked,
                    ObjectivesJson = aiStep.Objectives.Any() 
                        ? JsonSerializer.Serialize(aiStep.Objectives) 
                        : null
                };

                // Parse checkpoints per SRS 3.2.2
                foreach (var aiCheckpoint in aiStep.Checkpoints.OrderBy(c => c.OrderIndex))
                {
                    step.Checkpoints.Add(new StudyPlanCheckpoint
                    {
                        Description = aiCheckpoint.Description,
                        OrderIndex = aiCheckpoint.OrderIndex,
                        EstimatedMinutes = aiCheckpoint.EstimatedMinutes,
                        Type = aiCheckpoint.Type,
                        IsMandatory = aiCheckpoint.IsMandatory
                    });
                }

                studyPlan.Steps.Add(step);
            }

            // Parse resources per SRS 3.2.3
            foreach (var aiResource in aiResponse.Resources)
            {
                studyPlan.Resources.Add(new StudyPlanResource
                {
                    Title = aiResource.Title,
                    Url = aiResource.Url,
                    ResourceType = Enum.Parse<Domain.Entities.Assessment.ResourceType>(aiResource.ResourceType),
                    Description = aiResource.Description,
                    EstimatedHours = aiResource.EstimatedHours,
                    DifficultyLevel = !string.IsNullOrWhiteSpace(aiResource.DifficultyLevel)
                        ? Enum.Parse<Domain.Entities.Assessment.DifficultyLevel>(aiResource.DifficultyLevel)
                        : null,
                    IsFree = aiResource.IsFree,
                    Cost = aiResource.Cost,
                    Provider = aiResource.Provider,
                    Priority = aiResource.Priority
                });
            }

            // Parse skills per SRS 3.3
            foreach (var aiSkill in aiResponse.RequiredSkills)
            {
                var skill = await _skillRepo.GetOrCreateByNameAsync(aiSkill.SkillName, SkillCategory.Technical);
                
                studyPlan.RequiredSkills.Add(new StudyPlanSkill
                {
                    SkillId = skill.Id,
                    TargetProficiency = Enum.Parse<SkillLevel>(aiSkill.TargetProficiency),
                    Importance = aiSkill.Importance,
                    IsPrerequisite = aiSkill.IsPrerequisite,
                    Status = SkillStatus.Missing
                });
            }

            await _assessmentRepo.CreateStudyPlanAsync(studyPlan);
            return studyPlan;
        }

        private async Task PerformSkillGapAnalysisAsync(Guid userId, AiStudyPlan studyPlan)
        {
            var userProfile = await _profileRepo.GetByUserIdAsync(userId);
            if (userProfile == null) return;

            foreach (var requiredSkill in studyPlan.RequiredSkills)
            {
                var userSkill = userProfile.Skills.FirstOrDefault(us => us.SkillId == requiredSkill.SkillId);
                
                if (userSkill == null)
                {
                    requiredSkill.Status = SkillStatus.Missing;
                    requiredSkill.ProficiencyGap = (int)requiredSkill.TargetProficiency;
                }
                else
                {
                    var gap = (int)requiredSkill.TargetProficiency - (int)userSkill.ProficiencyLevel;
                    requiredSkill.ProficiencyGap = gap;
                    
                    if (gap <= 0)
                        requiredSkill.Status = SkillStatus.Achieved;
                    else
                        requiredSkill.Status = SkillStatus.InProgress;
                }
            }

            await _assessmentRepo.UpdateStudyPlanAsync(studyPlan);
        }

        private async Task RecalculateOverallProgressAsync(AiStudyPlan studyPlan)
        {
            if (!studyPlan.Steps.Any()) return;

            var totalProgress = studyPlan.Steps.Average(s => s.ProgressPercentage);
            studyPlan.ProgressPercentage = (int)totalProgress;

            if (studyPlan.ProgressPercentage >= 100)
            {
                studyPlan.Status = Domain.Entities.Assessment.StudyPlanStatus.Completed;
                studyPlan.CompletedAt = DateTime.UtcNow;
            }

            await _assessmentRepo.UpdateStudyPlanAsync(studyPlan);
        }

        private List<string> GenerateRecommendations(List<SkillGapDto> gaps)
        {
            var recommendations = new List<string>();

            var criticalGaps = gaps.Where(g => g.Priority == "High" && g.Status == "Missing").ToList();
            if (criticalGaps.Any())
            {
                recommendations.Add($"Focus on acquiring {criticalGaps.Count} high-priority missing skills first");
            }

            var achievedCount = gaps.Count(g => g.Status == "Achieved");
            if (achievedCount > 0)
            {
                recommendations.Add($"Great! You've already achieved {achievedCount} required skills");
            }

            return recommendations;
        }

        private StudyPlanDto MapToDto(AiStudyPlan studyPlan)
        {
            return new StudyPlanDto
            {
                Id = studyPlan.Id,
                UserId = studyPlan.UserId,
                AssessmentAttemptId = studyPlan.AssessmentAttemptId,
                CareerPlanId = studyPlan.CareerPlanId,
                Title = studyPlan.Title,
                Summary = studyPlan.Summary,
                TargetCompletionDate = studyPlan.TargetCompletionDate,
                EstimatedHours = studyPlan.EstimatedHours,
                DifficultyLevel = studyPlan.DifficultyLevel.ToString(),
                Status = studyPlan.Status.ToString(),
                ProgressPercentage = studyPlan.ProgressPercentage,
                StartedAt = studyPlan.StartedAt,
                CompletedAt = studyPlan.CompletedAt,
                IsActive = studyPlan.IsActive,
                AiModel = studyPlan.AiModel,
                CreatedAt = studyPlan.CreatedAt,
                Steps = studyPlan.Steps.Select(MapStepToDto).ToList(),
                RequiredSkills = studyPlan.RequiredSkills.Select(MapSkillToDto).ToList(),
                Resources = studyPlan.Resources.Select(MapResourceToDto).ToList()
            };
        }

        private StudyPlanStepDto MapStepToDto(StudyPlanStep step)
        {
            return new StudyPlanStepDto
            {
                Id = step.Id,
                Name = step.Name,
                Description = step.Description,
                OrderIndex = step.OrderIndex,
                EstimatedHours = step.EstimatedHours,
                Status = step.Status.ToString(),
                ProgressPercentage = step.ProgressPercentage,
                StartedAt = step.StartedAt,
                CompletedAt = step.CompletedAt,
                DueDate = step.DueDate,
                Objectives = !string.IsNullOrWhiteSpace(step.ObjectivesJson)
                    ? JsonSerializer.Deserialize<List<string>>(step.ObjectivesJson)
                    : null,
                Checkpoints = step.Checkpoints.Select(MapCheckpointToDto).ToList()
            };
        }

        private StudyPlanCheckpointDto MapCheckpointToDto(StudyPlanCheckpoint checkpoint)
        {
            return new StudyPlanCheckpointDto
            {
                Id = checkpoint.Id,
                Description = checkpoint.Description,
                OrderIndex = checkpoint.OrderIndex,
                EstimatedMinutes = checkpoint.EstimatedMinutes,
                IsCompleted = checkpoint.IsCompleted,
                CompletedAt = checkpoint.CompletedAt,
                Type = checkpoint.Type,
                IsMandatory = checkpoint.IsMandatory,
                Notes = checkpoint.Notes
            };
        }

        private StudyPlanResourceDto MapResourceToDto(StudyPlanResource resource)
        {
            return new StudyPlanResourceDto
            {
                Id = resource.Id,
                StudyPlanStepId = resource.StudyPlanStepId,
                Title = resource.Title,
                Url = resource.Url,
                ResourceType = resource.ResourceType.ToString(),
                Description = resource.Description,
                EstimatedHours = resource.EstimatedHours,
                DifficultyLevel = resource.DifficultyLevel?.ToString(),
                IsFree = resource.IsFree,
                Cost = resource.Cost,
                Provider = resource.Provider,
                Priority = resource.Priority,
                IsCompleted = resource.IsCompleted,
                UserRating = resource.UserRating
            };
        }

        private StudyPlanSkillDto MapSkillToDto(StudyPlanSkill skill)
        {
            return new StudyPlanSkillDto
            {
                SkillId = skill.SkillId,
                SkillName = skill.Skill.Name,
                TargetProficiency = skill.TargetProficiency.ToString(),
                Importance = skill.Importance,
                IsPrerequisite = skill.IsPrerequisite,
                Status = skill.Status.ToString(),
                ProficiencyGap = skill.ProficiencyGap
            };
        }
    }
}
