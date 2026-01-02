using Mentora.Domain.Common;
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
        public int DurationWeeks { get; set; } = 4;
        
        public CareerStepStatus Status { get; set; } = CareerStepStatus.NotStarted;
        public int ProgressPercentage { get; set; } = 0;
        
        public string ResourcesLinks { get; set; } = "[]";
        
        public ICollection<StudyTask> LinkedStudyTasks { get; set; } = new List<StudyTask>();
        public ICollection<CareerPlanSkill> Skills { get; set; } = new List<CareerPlanSkill>();
        
        [NotMapped]
        public int CalculatedProgress => CalculateProgress();
        
        private int CalculateProgress()
        {
            if (!Skills.Any()) return 0;
            
            var achievedSkills = Skills.Count(s => s.Status == SkillStatus.Achieved);
            var inProgressSkills = Skills.Count(s => s.Status == SkillStatus.InProgress);
            
            return (achievedSkills * 100 + inProgressSkills * 50) / Skills.Count;
        }
    }
}