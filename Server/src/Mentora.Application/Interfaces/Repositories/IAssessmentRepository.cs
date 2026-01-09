using Mentora.Domain.Entities.Assessment;

namespace Mentora.Application.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for Assessment module entities per SRS Section 3
    /// </summary>
    public interface IAssessmentRepository
    {
        // Assessment Questions per SRS 3.1.1
        Task<List<AssessmentQuestion>> GetQuestionsByMajorAsync(string major, CancellationToken cancellationToken = default);
        Task<List<AssessmentQuestion>> GetAllActiveQuestionsAsync(CancellationToken cancellationToken = default);
        Task<AssessmentQuestion?> GetQuestionByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<AssessmentQuestion> CreateQuestionAsync(AssessmentQuestion question, CancellationToken cancellationToken = default);
        Task UpdateQuestionAsync(AssessmentQuestion question, CancellationToken cancellationToken = default);
        Task DeleteQuestionAsync(Guid id, CancellationToken cancellationToken = default);

        // Assessment Attempts per SRS 3.1.2
        Task<AssessmentAttempt?> GetAttemptByIdAsync(Guid id, bool includeResponses = false, CancellationToken cancellationToken = default);
        Task<List<AssessmentAttempt>> GetUserAttemptsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<AssessmentAttempt?> GetActiveAttemptAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<AssessmentAttempt> CreateAttemptAsync(AssessmentAttempt attempt, CancellationToken cancellationToken = default);
        Task UpdateAttemptAsync(AssessmentAttempt attempt, CancellationToken cancellationToken = default);

        // Assessment Responses per SRS 3.1.2
        Task<List<AssessmentResponse>> GetAttemptResponsesAsync(Guid attemptId, CancellationToken cancellationToken = default);
        Task<AssessmentResponse?> GetResponseAsync(Guid attemptId, Guid questionId, CancellationToken cancellationToken = default);
        Task<AssessmentResponse> CreateResponseAsync(AssessmentResponse response, CancellationToken cancellationToken = default);
        Task UpdateResponseAsync(AssessmentResponse response, CancellationToken cancellationToken = default);
        Task BulkCreateResponsesAsync(List<AssessmentResponse> responses, CancellationToken cancellationToken = default);

        // Study Plans per SRS 3.2
        Task<AiStudyPlan?> GetStudyPlanByIdAsync(Guid id, bool includeAll = false, CancellationToken cancellationToken = default);
        Task<List<AiStudyPlan>> GetUserStudyPlansAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<AiStudyPlan?> GetActiveStudyPlanAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<AiStudyPlan> CreateStudyPlanAsync(AiStudyPlan studyPlan, CancellationToken cancellationToken = default);
        Task UpdateStudyPlanAsync(AiStudyPlan studyPlan, CancellationToken cancellationToken = default);
        Task DeleteStudyPlanAsync(Guid id, CancellationToken cancellationToken = default);

        // Study Plan Progress per SRS 5.2
        Task UpdateStepProgressAsync(Guid stepId, int progressPercentage, CancellationToken cancellationToken = default);
        Task UpdateCheckpointCompletionAsync(Guid checkpointId, bool isCompleted, CancellationToken cancellationToken = default);
        Task UpdateResourceCompletionAsync(Guid resourceId, bool isCompleted, int? userRating = null, CancellationToken cancellationToken = default);
    }
}
