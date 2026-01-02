using Mentora.Application.DTOs.CareerBuilder;
using Mentora.Application.Interfaces.Repositories;
using Mentora.Application.Interfaces.Services;
using Mentora.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentora.Application.Services
{
    /// <summary>
    /// Service implementation for Career Plans per SRS Feature 1
    /// </summary>
    public class CareerPlanService : ICareerPlanService
    {
        private readonly ICareerPlanRepository _planRepository;
        private readonly ICareerStepRepository _stepRepository;
        private readonly ICareerPlanSkillRepository _skillRepository;
        private readonly ICareerQuizRepository _quizRepository;
        private readonly IAiCareerService _aiService;
        private readonly ISkillRepository _masterSkillRepository;

        public CareerPlanService(
            ICareerPlanRepository planRepository,
            ICareerStepRepository stepRepository,
            ICareerPlanSkillRepository skillRepository,
            ICareerQuizRepository quizRepository,
            IAiCareerService aiService,
            ISkillRepository masterSkillRepository)
        {
            _planRepository = planRepository;
            _stepRepository = stepRepository;
            _skillRepository = skillRepository;
            _quizRepository = quizRepository;
            _aiService = aiService;
            _masterSkillRepository = masterSkillRepository;
        }

        public async Task<CareerPlanGeneratedDto> GenerateCareerPlanAsync(Guid userId, GenerateCareerPlanDto dto)
        {
            try
            {
                // Get quiz attempt
                var quiz = await _quizRepository.GetByIdAsync(dto.QuizAttemptId);
                if (quiz == null || quiz.UserId != userId)
                {
                    return new CareerPlanGeneratedDto
                    {
                        Success = false,
                        Message = "Quiz attempt not found or access denied"
                    };
                }

                if (quiz.Status != CareerQuizStatus.Completed)
                {
                    return new CareerPlanGeneratedDto
                    {
                        Success = false,
                        Message = "Quiz must be completed before generating a career plan"
                    };
                }

                // Call AI to generate career plan per SRS Section 7
                var aiResponse = await _aiService.GenerateCareerPlanAsync(quiz.AnswersJson);

                // Create Career Plan per SRS FR-CP-01 using existing schema
                var plan = new CareerPlan
                {
                    UserId = userId,
                    Title = aiResponse.Title,
                    Summary = aiResponse.Summary,
                    TargetRole = ExtractTargetRole(aiResponse.Title),
                    Description = aiResponse.Summary,
                    TimelineMonths = CalculateTimelineMonths(aiResponse.Steps.Count),
                    CurrentStepIndex = 0,
                    IsActive = true,
                    Status = CareerPlanStatus.Generated,
                    CreatedAt = DateTime.UtcNow
                };

                plan = await _planRepository.CreateAsync(plan);

                // Create Steps per SRS FR-CP-03 using existing schema
                var steps = aiResponse.Steps.Select(s => new CareerStep
                {
                    CareerPlanId = plan.Id,
                    Title = s.Name,
                    Description = s.Description,
                    OrderIndex = s.OrderIndex,
                    DurationWeeks = 4, // Default 4 weeks per step
                    Status = CareerStepStatus.NotStarted,
                    ResourcesLinks = "[]"
                }).ToList();

                await _stepRepository.CreateManyAsync(steps);

                // Create Skills per SRS FR-SK-01 & 5.6 using existing schema
                var skills = new List<CareerPlanSkill>();

                foreach (var aiSkill in aiResponse.Skills)
                {
                    // Find or create skill
                    var skill = await GetOrCreateSkillAsync(aiSkill.Name);

                    skills.Add(new CareerPlanSkill
                    {
                        CareerPlanId = plan.Id,
                        SkillId = skill.Id,
                        TargetLevel = DetermineSkillLevel(aiSkill.Category)
                    });
                }

                await _skillRepository.CreateManyAsync(skills);

                // Return generated plan details
                var planDetail = await GetPlanDetailsAsync(userId, plan.Id);

                return new CareerPlanGeneratedDto
                {
                    Success = true,
                    Message = "Career plan generated successfully",
                    CareerPlan = planDetail
                };
            }
            catch (Exception ex)
            {
                return new CareerPlanGeneratedDto
                {
                    Success = false,
                    Message = $"Error generating career plan: {ex.Message}"
                };
            }
        }

        public async Task<List<CareerPlanListDto>> GetAllPlansAsync(Guid userId)
        {
            var plans = await _planRepository.GetAllByUserIdAsync(userId);

            return plans.Select(p => new CareerPlanListDto
            {
                Id = p.Id,
                Title = p.Title,
                Status = p.Status.ToString(),
                ProgressPercentage = CalculateProgressPercentage(p),
                CreatedAt = p.CreatedAt
            }).ToList();
        }

        public async Task<CareerPlanDetailDto?> GetPlanDetailsAsync(Guid userId, Guid planId)
        {
            var plan = await _planRepository.GetByIdAndUserIdAsync(planId, userId);
            
            if (plan == null)
                return null;

            // Get plan with steps
            var planWithSteps = await _planRepository.GetByIdWithStepsAsync(planId);
            
            if (planWithSteps == null)
                return null;

            return new CareerPlanDetailDto
            {
                Id = planWithSteps.Id,
                Title = planWithSteps.Title,
                Summary = planWithSteps.Summary,
                Status = planWithSteps.Status.ToString(),
                ProgressPercentage = CalculateProgressPercentage(planWithSteps),
                CreatedAt = planWithSteps.CreatedAt,
                Steps = planWithSteps.Steps
                    .OrderBy(s => s.OrderIndex)
                    .Select(s => new CareerStepDto
                    {
                        Id = s.Id,
                        Name = s.Title,
                        Description = s.Description,
                        OrderIndex = s.OrderIndex,
                        ProgressPercentage = CalculateStepProgressPercentage(s)
                    })
                    .ToList()
            };
        }

        #region Private Helper Methods

        private async Task<Skill> GetOrCreateSkillAsync(string name)
        {
            // Check if skill already exists
            var existingSkill = await _masterSkillRepository.GetByNameAsync(name);
            if (existingSkill != null)
                return existingSkill;

            // Create new skill
            var newSkill = new Skill
            {
                Name = name
            };

            return await _masterSkillRepository.CreateAsync(newSkill);
        }

        private string ExtractTargetRole(string title)
        {
            // Extract role from title like "Career Path to Senior Developer"
            var parts = title.Split("to", StringSplitOptions.TrimEntries);
            return parts.Length > 1 ? parts[1] : title;
        }

        private int CalculateTimelineMonths(int stepCount)
        {
            // Each step is ~4 weeks, so calculate total months
            return (stepCount * 4) / 4; // 4 steps = 4 months average
        }

        private SkillLevel DetermineSkillLevel(string category)
        {
            // Technical skills typically need intermediate level, soft skills beginner
            return category.Equals("Technical", StringComparison.OrdinalIgnoreCase)
                ? SkillLevel.Intermediate
                : SkillLevel.Beginner;
        }

        private int CalculateProgressPercentage(CareerPlan plan)
        {
            if (!plan.Steps.Any())
                return 0;

            var completedSteps = plan.Steps.Count(s => s.Status == CareerStepStatus.Completed);
            return (int)((completedSteps / (double)plan.Steps.Count) * 100);
        }

        private int CalculateStepProgressPercentage(CareerStep step)
        {
            // Calculate based on step status
            return step.Status switch
            {
                CareerStepStatus.NotStarted => 0,
                CareerStepStatus.InProgress => 50,
                CareerStepStatus.Completed => 100,
                _ => 0
            };
        }

        #endregion
    }
}
