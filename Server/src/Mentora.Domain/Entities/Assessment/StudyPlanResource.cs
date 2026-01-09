using Mentora.Domain.Common;

namespace Mentora.Domain.Entities.Assessment
{
    /// <summary>
    /// Learning resources extracted from AI response per SRS 3.2.3
    /// Includes courses, articles, videos, documentation, etc.
    /// </summary>
    public class StudyPlanResource : BaseEntity
    {
        /// <summary>
        /// Parent study plan
        /// </summary>
        public Guid StudyPlanId { get; set; }
        public AiStudyPlan StudyPlan { get; set; } = null!;

        /// <summary>
        /// Optional association with a specific step
        /// </summary>
        public Guid? StudyPlanStepId { get; set; }
        public StudyPlanStep? StudyPlanStep { get; set; }

        /// <summary>
        /// Resource title
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Resource URL/link
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Type of resource (Course, Article, Video, Book, Documentation, Tutorial)
        /// </summary>
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// Brief description of the resource
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Estimated time to complete in hours
        /// </summary>
        public int? EstimatedHours { get; set; }

        /// <summary>
        /// Difficulty level
        /// </summary>
        public DifficultyLevel? DifficultyLevel { get; set; }

        /// <summary>
        /// Whether this resource is free
        /// </summary>
        public bool IsFree { get; set; } = true;

        /// <summary>
        /// Cost in USD if not free
        /// </summary>
        public decimal? Cost { get; set; }

        /// <summary>
        /// Platform/provider (e.g., "Coursera", "Udemy", "YouTube", "Official Docs")
        /// </summary>
        public string? Provider { get; set; }

        /// <summary>
        /// Priority/importance (1-5, 5 being highest)
        /// </summary>
        public int Priority { get; set; } = 3;

        /// <summary>
        /// Whether user has completed this resource
        /// </summary>
        public bool IsCompleted { get; set; } = false;

        /// <summary>
        /// When the user completed this resource
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// User rating (1-5 stars)
        /// </summary>
        public int? UserRating { get; set; }

        /// <summary>
        /// User notes about this resource
        /// </summary>
        public string? UserNotes { get; set; }

        /// <summary>
        /// Tags for categorization (JSON array)
        /// </summary>
        public string? TagsJson { get; set; }
    }

    /// <summary>
    /// Resource type enum for categorization
    /// </summary>
    public enum ResourceType
    {
        Course = 0,
        Article = 1,
        Video = 2,
        Book = 3,
        Documentation = 4,
        Tutorial = 5,
        Practice = 6,
        Project = 7,
        Tool = 8,
        Community = 9
    }
}
