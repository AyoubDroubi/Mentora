using CleanBackend.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Mentora.Domain.Entities
{
    public class TaskFeedbackLog : BaseEntity
    {
        public Guid StudyTaskId { get; set; }
        public StudyTask StudyTask { get; set; } = null!;
        public string FailureReason { get; set; } = string.Empty;
        public string Mood { get; set; } = "Neutral";
        public string UserComment { get; set; } = string.Empty;
    }
}