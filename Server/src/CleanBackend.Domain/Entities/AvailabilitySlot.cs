using System.ComponentModel.DataAnnotations;

namespace Mentora.Domain.Entities
{
    public class AvailabilitySlot
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // تابعة لخطة دراسية معينة
        public Guid StudyPlanId { get; set; }
        public StudyPlan StudyPlan { get; set; } = null!;

        public DayOfWeek Day { get; set; } // Monday, Tuesday...

        // فترة متاحة من الساعة 18:00 إلى 20:00
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        // نوع الوقت: "High Focus" (صباح) ولا "Low Focus" (ليل)
        // عشان السيستم يحط المواد الصعبة في الـ High Focus
        public string EnergyLevel { get; set; } = "High";
    }
}