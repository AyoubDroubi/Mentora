using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentora.Domain.Entities
{
    public class CareerPlan
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        [Required, MaxLength(150)]
        public string Title { get; set; } = string.Empty; // e.g. "React Developer Path"

        [MaxLength(1000)]
        public string Summary { get; set; } = string.Empty; // AI Summary

        public PlanStatus Status { get; set; } = PlanStatus.Active;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // الخطوات التفصيلية للخطة
        public ICollection<CareerStep> Steps { get; set; } = new List<CareerStep>();
    }
}
