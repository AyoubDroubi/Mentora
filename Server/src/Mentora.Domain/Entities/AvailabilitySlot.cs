using Mentora.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Mentora.Domain.Entities
{
    public class AvailabilitySlot : BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid StudyPlanId { get; set; }
        public StudyPlan StudyPlan { get; set; } = null!;
        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string EnergyLevel { get; set; } = "High";
    }
}