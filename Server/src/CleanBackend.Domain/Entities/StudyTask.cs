using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentora.Domain.Entities
{
    public class StudyTask
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public TaskFeedbackLog? FeedbackLog { get; set; }
        // --- الرابط الذهبي (اختياري) ---
        // ممكن التاسك يكون لدراسة خطوة كرير، وممكن يكون امتحان جامعة عادي
        [ForeignKey("CareerStep")]
        public Guid? CareerStepId { get; set; }
        public CareerStep? CareerStep { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Subject { get; set; } = "General"; // e.g. "Math", "React"

        public DateTime ScheduledDate { get; set; } // اليوم
        public int DurationMinutes { get; set; } = 60; // 30, 45, 60

        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public TaskStatus Status { get; set; } = TaskStatus.Pending;

        public string? Notes { get; set; } // ملاحظات الطالب عالتاسك
    }
}
