using Mentora.Domain.Common;
using Mentora.Domain.Entities.Assessment;
using Mentora.Domain.Entities.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentora.Domain.Entities
{
    public class StudyTask : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public TaskFeedbackLog? FeedbackLog { get; set; }

        // Legacy career plan integration per SRS 5.1.1
        public Guid? CareerStepId { get; set; }

        public CareerStep? CareerStep { get; set; }

        // New study plan integration per SRS 5.1
        public Guid? StudyPlanStepId { get; set; }

        public StudyPlanStep? StudyPlanStep { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Subject { get; set; } = "General";
        public DateTime ScheduledDate { get; set; }
        public int DurationMinutes { get; set; } = 60;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public TaskStatus Status { get; set; } = TaskStatus.Pending;
        public Guid? StudyPlanId { get; set; }
        public StudyPlan? StudyPlan { get; set; }
        public TimeSpan? StartTime { get; set; }
        public string? Notes { get; set; }
    }
}