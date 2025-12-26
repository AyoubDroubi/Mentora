using CleanBackend.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentora.Domain.Entities
{
    public class CareerStep : BaseEntity
    {
        public Guid CareerPlanId { get; set; }
        public CareerPlan CareerPlan { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
        public StepStatus Status { get; set; } = StepStatus.Locked;
        public string ResourcesLinks { get; set; } = "[]";
        public ICollection<StudyTask> LinkedStudyTasks { get; set; } = new List<StudyTask>();
    }
}