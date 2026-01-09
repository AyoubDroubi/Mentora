using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities;
using Mentora.Domain.Entities.Assessment;
using Mentora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Mentora.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for Assessment module per SRS Section 3
    /// Handles all database operations for assessment questions, attempts, responses, and study plans
    /// </summary>
    public class AssessmentRepository : IAssessmentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AssessmentRepository> _logger;

        public AssessmentRepository(
            ApplicationDbContext context,
            ILogger<AssessmentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Assessment Questions per SRS 3.1.1

        public async Task<List<AssessmentQuestion>> GetQuestionsByMajorAsync(
            string major,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"?? Retrieving questions for major: {major}");

            return await _context.AssessmentQuestions
                .Where(q => q.IsActive && (q.TargetMajor == major || q.TargetMajor == null))
                .OrderBy(q => q.OrderIndex)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<List<AssessmentQuestion>> GetAllActiveQuestionsAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.AssessmentQuestions
                .Where(q => q.IsActive)
                .OrderBy(q => q.OrderIndex)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<AssessmentQuestion?> GetQuestionByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _context.AssessmentQuestions
                .FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
        }

        public async Task<AssessmentQuestion> CreateQuestionAsync(
            AssessmentQuestion question,
            CancellationToken cancellationToken = default)
        {
            _context.AssessmentQuestions.Add(question);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"? Question {question.Id} created");
            return question;
        }

        public async Task UpdateQuestionAsync(
            AssessmentQuestion question,
            CancellationToken cancellationToken = default)
        {
            _context.AssessmentQuestions.Update(question);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"? Question {question.Id} updated");
        }

        public async Task DeleteQuestionAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var question = await GetQuestionByIdAsync(id, cancellationToken);
            if (question != null)
            {
                _context.AssessmentQuestions.Remove(question);
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation($"??? Question {id} deleted");
            }
        }

        #endregion

        #region Assessment Attempts per SRS 3.1.2

        public async Task<AssessmentAttempt?> GetAttemptByIdAsync(
            Guid id,
            bool includeResponses = false,
            CancellationToken cancellationToken = default)
        {
            var query = _context.AssessmentAttempts
                .Where(a => a.Id == id);

            if (includeResponses)
            {
                query = query
                    .Include(a => a.Responses)
                        .ThenInclude(r => r.Question);
            }

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<AssessmentAttempt>> GetUserAttemptsAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await _context.AssessmentAttempts
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CreatedAt)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<AssessmentAttempt?> GetActiveAttemptAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await _context.AssessmentAttempts
                .Where(a => a.UserId == userId && a.Status == AssessmentStatus.Draft)
                .OrderByDescending(a => a.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<AssessmentAttempt> CreateAttemptAsync(
            AssessmentAttempt attempt,
            CancellationToken cancellationToken = default)
        {
            _context.AssessmentAttempts.Add(attempt);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"?? Assessment attempt {attempt.Id} created for user {attempt.UserId}");
            return attempt;
        }

        public async Task UpdateAttemptAsync(
            AssessmentAttempt attempt,
            CancellationToken cancellationToken = default)
        {
            _context.AssessmentAttempts.Update(attempt);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"? Assessment attempt {attempt.Id} updated");
        }

        #endregion

        #region Assessment Responses per SRS 3.1.2

        public async Task<List<AssessmentResponse>> GetAttemptResponsesAsync(
            Guid attemptId,
            CancellationToken cancellationToken = default)
        {
            return await _context.AssessmentResponses
                .Include(r => r.Question)
                .Where(r => r.AssessmentAttemptId == attemptId)
                .OrderBy(r => r.Question.OrderIndex)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<AssessmentResponse?> GetResponseAsync(
            Guid attemptId,
            Guid questionId,
            CancellationToken cancellationToken = default)
        {
            return await _context.AssessmentResponses
                .FirstOrDefaultAsync(
                    r => r.AssessmentAttemptId == attemptId && r.QuestionId == questionId,
                    cancellationToken);
        }

        public async Task<AssessmentResponse> CreateResponseAsync(
            AssessmentResponse response,
            CancellationToken cancellationToken = default)
        {
            _context.AssessmentResponses.Add(response);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"?? Response created for question {response.QuestionId}");
            return response;
        }

        public async Task UpdateResponseAsync(
            AssessmentResponse response,
            CancellationToken cancellationToken = default)
        {
            _context.AssessmentResponses.Update(response);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"? Response updated for question {response.QuestionId}");
        }

        public async Task BulkCreateResponsesAsync(
            List<AssessmentResponse> responses,
            CancellationToken cancellationToken = default)
        {
            _context.AssessmentResponses.AddRange(responses);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"?? Bulk created {responses.Count} responses");
        }

        #endregion

        #region Study Plans per SRS 3.2

        public async Task<AiStudyPlan?> GetStudyPlanByIdAsync(
            Guid id,
            bool includeAll = false,
            CancellationToken cancellationToken = default)
        {
            var query = _context.AiStudyPlans
                .Where(p => p.Id == id);

            if (includeAll)
            {
                query = query
                    .Include(p => p.Steps.OrderBy(s => s.OrderIndex))
                        .ThenInclude(s => s.Checkpoints.OrderBy(c => c.OrderIndex))
                    .Include(p => p.RequiredSkills)
                        .ThenInclude(s => s.Skill)
                    .Include(p => p.Resources.OrderBy(r => r.Priority));
            }

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<AiStudyPlan>> GetUserStudyPlansAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await _context.AiStudyPlans
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<AiStudyPlan?> GetActiveStudyPlanAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await _context.AiStudyPlans
                .Include(p => p.Steps.OrderBy(s => s.OrderIndex))
                    .ThenInclude(s => s.Checkpoints.OrderBy(c => c.OrderIndex))
                .Include(p => p.RequiredSkills)
                    .ThenInclude(s => s.Skill)
                .Include(p => p.Resources)
                .Where(p => p.UserId == userId && p.IsActive)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<AiStudyPlan> CreateStudyPlanAsync(
            AiStudyPlan studyPlan,
            CancellationToken cancellationToken = default)
        {
            _context.AiStudyPlans.Add(studyPlan);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"?? Study plan {studyPlan.Id} created with {studyPlan.Steps.Count} steps");
            return studyPlan;
        }

        public async Task UpdateStudyPlanAsync(
            AiStudyPlan studyPlan,
            CancellationToken cancellationToken = default)
        {
            _context.AiStudyPlans.Update(studyPlan);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"? Study plan {studyPlan.Id} updated");
        }

        public async Task DeleteStudyPlanAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var studyPlan = await GetStudyPlanByIdAsync(id, includeAll: false, cancellationToken);
            if (studyPlan != null)
            {
                _context.AiStudyPlans.Remove(studyPlan);
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation($"??? Study plan {id} deleted");
            }
        }

        #endregion

        #region Progress Tracking per SRS 5.2

        public async Task UpdateStepProgressAsync(
            Guid stepId,
            int progressPercentage,
            CancellationToken cancellationToken = default)
        {
            var step = await _context.StudyPlanSteps.FindAsync(new object[] { stepId }, cancellationToken);
            if (step != null)
            {
                step.ProgressPercentage = progressPercentage;
                
                // Update status based on progress
                if (progressPercentage == 0)
                    step.Status = StepStatus.NotStarted;
                else if (progressPercentage < 100)
                {
                    step.Status = StepStatus.InProgress;
                    step.StartedAt ??= DateTime.UtcNow;
                }
                else
                {
                    step.Status = StepStatus.Completed;
                    step.CompletedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation($"?? Step {stepId} progress updated to {progressPercentage}%");
            }
        }

        public async Task UpdateCheckpointCompletionAsync(
            Guid checkpointId,
            bool isCompleted,
            CancellationToken cancellationToken = default)
        {
            var checkpoint = await _context.StudyPlanCheckpoints.FindAsync(new object[] { checkpointId }, cancellationToken);
            if (checkpoint != null)
            {
                checkpoint.IsCompleted = isCompleted;
                checkpoint.CompletedAt = isCompleted ? DateTime.UtcNow : null;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation($"? Checkpoint {checkpointId} marked as {(isCompleted ? "completed" : "incomplete")}");
            }
        }

        public async Task UpdateResourceCompletionAsync(
            Guid resourceId,
            bool isCompleted,
            int? userRating = null,
            CancellationToken cancellationToken = default)
        {
            var resource = await _context.StudyPlanResources.FindAsync(new object[] { resourceId }, cancellationToken);
            if (resource != null)
            {
                resource.IsCompleted = isCompleted;
                resource.CompletedAt = isCompleted ? DateTime.UtcNow : null;
                
                if (userRating.HasValue && userRating.Value >= 1 && userRating.Value <= 5)
                {
                    resource.UserRating = userRating.Value;
                }

                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation($"?? Resource {resourceId} marked as {(isCompleted ? "completed" : "incomplete")}");
            }
        }

        #endregion
    }
}
