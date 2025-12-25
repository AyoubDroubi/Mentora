using System.ComponentModel.DataAnnotations;

namespace Mentora.Domain.Entities
{
    public class DailyReflection
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }

        public DateTime Date { get; set; }

        // 1-10
        public int SatisfactionScore { get; set; }

        // "I felt productive today but got tired in the evening..."
        public string Summary { get; set; } = string.Empty;
    }
}