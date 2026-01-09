using Mentora.Domain.Common;

namespace Mentora.Domain.Entities.Assessment
{
    /// <summary>
    /// Milestones in the study plan per SRS 3.2.2
    /// Represents major phases or modules in the learning journey
    /// </summary>
    public class StudyPlanStep : BaseEntity
    {
        /// <summary>
        /// Parent study plan
        /// </summary>
        public Guid StudyPlanId { get; set; }
        public AiStudyPlan StudyPlan { get; set; } = null!;

        /// <summary>
        /// Step name/title
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of what this step covers
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Order in the study plan sequence
        /// </summary>
        public int OrderIndex { get; set; }

        /// <summary>
        /// Estimated duration in hours
        /// </summary>
        public int EstimatedHours { get; set; }

        /// <summary>
        /// Current status
        /// </summary>
        public StepStatus Status { get; set; } = StepStatus.Locked;

        /// <summary>
        /// Progress percentage (0-100) per SRS 5.2.1
        /// </summary>
        public int ProgressPercentage { get; set; } = 0;

        /// <summary>
        /// When the step was started
        /// </summary>
        public DateTime? StartedAt { get; set; }

        /// <summary>
        /// When the step was completed
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// Optional due date for this step
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Learning objectives as JSON array
        /// </summary>
        public string? ObjectivesJson { get; set; }

        /// <summary>
        /// Prerequisites as JSON array (other step IDs or skill names)
        /// </summary>
        public string? PrerequisitesJson { get; set; }

        // Navigation properties
        public ICollection<StudyPlanCheckpoint> Checkpoints { get; set; } = new List<StudyPlanCheckpoint>();
        public ICollection<StudyTask> Tasks { get; set; } = new List<StudyTask>(); // Integration per SRS 5.1
    }
}
