using System.ComponentModel.DataAnnotations;

namespace Mentora.Domain.Entities
{
    public class StudySession
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // تابعة لأي تاسك؟ (اختياري، ممكن يدرس بدون تاسك)
        public Guid? StudyTaskId { get; set; }
        public StudyTask? StudyTask { get; set; }

        public Guid UserId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; } // Nullable يعني لسا الجلسة شغالة

        public int DurationMinutes { get; set; } // بنحسبها لما يخلص

        // من 1 لـ 100: بناءً على هل التزم بالوقت او ضل يوقف المؤقت
        public int FocusScore { get; set; } = 100;
    }
}