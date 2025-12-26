using Mentora.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Mentora.Domain.Entities
{
    public class DailyReflection : BaseEntity
    {
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public int SatisfactionScore { get; set; }
        public string Summary { get; set; } = string.Empty;
    }
}