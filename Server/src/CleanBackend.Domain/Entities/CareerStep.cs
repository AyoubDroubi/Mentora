using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentora.Domain.Entities
{
    public class CareerStep
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [ForeignKey("CareerPlan")]
        public Guid CareerPlanId { get; set; }
        public CareerPlan CareerPlan { get; set; } = null!;

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty; // e.g. "Learn Hooks"

        public string Description { get; set; } = string.Empty; // Detailed MD

        public int OrderIndex { get; set; } // 1, 2, 3

        public StepStatus Status { get; set; } = StepStatus.Locked;

        public string ResourcesLinks { get; set; } = "[]"; // JSON Array of URLs

        // العلاقة العكسية مع التاسكات اليومية
        public ICollection<StudyTask> LinkedStudyTasks { get; set; } = new List<StudyTask>();
    }
}
