using CleanBackend.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Mentora.Domain.Entities
{
    public class AiRequestLog : BaseEntity
    {
        public Guid UserId { get; set; }
        public string RequestType { get; set; } = "CAREER_GEN";
        public int TokenUsage { get; set; }
        public bool IsSuccess { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}