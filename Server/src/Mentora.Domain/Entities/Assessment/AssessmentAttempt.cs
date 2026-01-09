using Mentora.Domain.Common;
using Mentora.Domain.Entities.Auth;

namespace Mentora.Domain.Entities.Assessment
{
    /// <summary>
    /// Represents a single assessment attempt by a user per SRS 3.1
    /// Aggregates all responses for AI context building
    /// </summary>
    public class AssessmentAttempt : BaseEntity
    {
        /// <summary>
        /// User taking the assessment
        /// </summary>
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        /// <summary>
        /// User's major at the time of assessment
        /// </summary>
        public string Major { get; set; } = string.Empty;

        /// <summary>
        /// Current study level (Freshman, Sophomore, etc.)
        /// </summary>
        public StudyLevel StudyLevel { get; set; }

        /// <summary>
        /// Assessment status
        /// </summary>
        public AssessmentStatus Status { get; set; } = AssessmentStatus.Draft;

        /// <summary>
        /// When the assessment was started
        /// </summary>
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// When the assessment was completed/submitted
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// Total time taken in minutes
        /// </summary>
        public int? TotalTimeMinutes { get; set; }

        /// <summary>
        /// Completion percentage (0-100)
        /// </summary>
        public int CompletionPercentage { get; set; } = 0;

        /// <summary>
        /// Serialized JSON context for AI injection per SRS 3.1.2
        /// Contains aggregated user responses in simplified format
        /// </summary>
        public string? ContextJson { get; set; }

        /// <summary>
        /// Additional metadata as JSON
        /// </summary>
        public string? MetadataJson { get; set; }

        // Navigation properties
        public ICollection<AssessmentResponse> Responses { get; set; } = new List<AssessmentResponse>();
        public ICollection<AiStudyPlan> GeneratedStudyPlans { get; set; } = new List<AiStudyPlan>();
    }

    /// <summary>
    /// Assessment attempt status per SRS design
    /// </summary>
    public enum AssessmentStatus
    {
        /// <summary>
        /// Assessment started but not completed
        /// </summary>
        Draft = 0,

        /// <summary>
        /// Assessment completed and submitted
        /// </summary>
        Completed = 1,

        /// <summary>
        /// Assessment expired or outdated (e.g., user profile changed significantly)
        /// </summary>
        Outdated = 2,

        /// <summary>
        /// Assessment abandoned by user
        /// </summary>
        Abandoned = 3
    }
}
