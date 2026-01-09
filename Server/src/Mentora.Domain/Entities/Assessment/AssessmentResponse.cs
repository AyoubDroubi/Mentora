using Mentora.Domain.Common;
using Mentora.Domain.Entities.Auth;

namespace Mentora.Domain.Entities.Assessment
{
    /// <summary>
    /// User's response to an assessment question per SRS 3.1.2
    /// Supports data serialization for AI prompt injection
    /// </summary>
    public class AssessmentResponse : BaseEntity
    {
        /// <summary>
        /// User who provided the response
        /// </summary>
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        /// <summary>
        /// Assessment attempt this response belongs to
        /// </summary>
        public Guid AssessmentAttemptId { get; set; }
        public AssessmentAttempt AssessmentAttempt { get; set; } = null!;

        /// <summary>
        /// Question being answered
        /// </summary>
        public Guid QuestionId { get; set; }
        public AssessmentQuestion Question { get; set; } = null!;

        /// <summary>
        /// User's answer as string (JSON for complex answers)
        /// </summary>
        public string ResponseValue { get; set; } = string.Empty;

        /// <summary>
        /// Time taken to answer this question in seconds
        /// </summary>
        public int? ResponseTimeSeconds { get; set; }

        /// <summary>
        /// Whether the user skipped this question
        /// </summary>
        public bool IsSkipped { get; set; } = false;

        /// <summary>
        /// Optional notes or comments from the user
        /// </summary>
        public string? Notes { get; set; }
    }
}
