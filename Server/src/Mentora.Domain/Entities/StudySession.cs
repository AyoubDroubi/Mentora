using CleanBackend.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Mentora.Domain.Entities
{
    public class StudySession : BaseEntity
    {
        public Guid? StudyTaskId { get; set; }
        public StudyTask? StudyTask { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int DurationMinutes { get; set; }
        public int PauseCount { get; set; } = 0;
        public int FocusScore { get; set; } = 100;
    }
}