using CleanBackend.Domain.Common;
using CleanBackend.Domain.Entities.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentora.Domain.Entities
{
    public class StudyTask : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public TaskFeedbackLog? FeedbackLog { get; set; }
        public Guid? CareerStepId { get; set; }
        public CareerStep? CareerStep { get; set; }
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