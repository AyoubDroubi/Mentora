using System.ComponentModel.DataAnnotations;

namespace Mentora.Domain.Entities
{
    public class TaskFeedbackLog
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid StudyTaskId { get; set; }

        // بنستعمل One-to-One هون
        public StudyTask StudyTask { get; set; } = null!;

        // Reason: "Procrastination", "Too Hard", "Unclear", "Unexpected Event"
        public string FailureReason { get; set; } = string.Empty;

        // كيف كان شعوره؟ (متحمس، زهقان، محبط)
        public string Mood { get; set; } = "Neutral";

        public string UserComment { get; set; } = string.Empty; // شرح حر
    }
}