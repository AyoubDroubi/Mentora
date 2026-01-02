using Mentora.Domain.Common;
using Mentora.Domain.Entities.Auth;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentora.Domain.Entities
{
    public class CareerPlan : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        
        public Guid? CareerQuizAttemptId { get; set; }
        public CareerQuizAttempt? CareerQuizAttempt { get; set; }
        
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string TargetRole { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        public CareerPlanStatus Status { get; set; } = CareerPlanStatus.Generated;
        public int ProgressPercentage { get; set; } = 0;
        
        public int TimelineMonths { get; set; } = 12;
        public int CurrentStepIndex { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        public ICollection<CareerStep> Steps { get; set; } = new List<CareerStep>();
        public ICollection<CareerPlanSkill> Skills { get; set; } = new List<CareerPlanSkill>();
        
        [NotMapped]
        public int CalculatedProgress => CalculateProgress();
        
        private int CalculateProgress()
        {
            if (!Steps.Any()) return 0;
            
            var stepsWithSkills = Steps.Where(s => s.Skills.Any()).ToList();
            if (!stepsWithSkills.Any()) return 0;
            
            var totalProgress = stepsWithSkills.Sum(s => s.CalculatedProgress);
            return totalProgress / stepsWithSkills.Count;
        }
    }
}