using Mentora.Domain.Common;

namespace Mentora.Domain.Entities.Assessment
{
    /// <summary>
    /// Granular micro-tasks within a study step per SRS 3.2.2
    /// Examples: "Install VS Code", "Complete Chapter 3 exercises"
    /// </summary>
    public class StudyPlanCheckpoint : BaseEntity
    {
        /// <summary>
        /// Parent study plan step
        /// </summary>
        public Guid StudyPlanStepId { get; set; }
        public StudyPlanStep StudyPlanStep { get; set; } = null!;

        /// <summary>
        /// Checkpoint description/title
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Order within the step
        /// </summary>
        public int OrderIndex { get; set; }

        /// <summary>
        /// Estimated duration in minutes
        /// </summary>
        public int EstimatedMinutes { get; set; }

        /// <summary>
        /// Whether this checkpoint is completed
        /// </summary>
        public bool IsCompleted { get; set; } = false;

        /// <summary>
        /// When the checkpoint was completed
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// Optional link to a study task for execution per SRS 5.1.2
        /// </summary>
        public Guid? StudyTaskId { get; set; }
        public StudyTask? StudyTask { get; set; }

        /// <summary>
        /// Checkpoint type (e.g., "Setup", "Reading", "Exercise", "Project")
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Whether this is a mandatory checkpoint
        /// </summary>
        public bool IsMandatory { get; set; } = true;

        /// <summary>
        /// Additional notes or instructions
        /// </summary>
        public string? Notes { get; set; }
    }
}
