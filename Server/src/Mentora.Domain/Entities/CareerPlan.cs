using Mentora.Domain.Common;
using Mentora.Domain.Entities.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentora.Domain.Entities
{
    public class CareerPlan : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string TargetRole { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TimelineMonths { get; set; } = 12;
        public int CurrentStepIndex { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public string Summary { get; set; } = string.Empty;
        public PlanStatus Status { get; set; } = PlanStatus.Active;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<CareerStep> Steps { get; set; } = new List<CareerStep>();
    }
}