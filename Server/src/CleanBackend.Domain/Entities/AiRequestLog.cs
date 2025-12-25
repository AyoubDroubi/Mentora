using System.ComponentModel.DataAnnotations;

namespace Mentora.Domain.Entities
{
    public class AiRequestLog
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }

        public string RequestType { get; set; } = "CAREER_GEN"; // CAREER or STUDY

        public int TokenUsage { get; set; } // للتكلفة
        public bool IsSuccess { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}