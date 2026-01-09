using Mentora.Domain.Common;

namespace Mentora.Domain.Entities
{
    public class Skill : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public SkillCategory Category { get; set; } = SkillCategory.Technical;
        public string Description { get; set; } = string.Empty; // Added per SRS 3.3.2
        
        // Navigation Properties
        /// <summary>
        /// User Profile Skills - Many-to-Many relationship
        /// </summary>
        public ICollection<UserProfileSkill> UserProfileSkills { get; set; } = new List<UserProfileSkill>();
    }

    public class CareerPlanSkill : BaseEntity
    {
        public Guid CareerPlanId { get; set; }
        public CareerPlan CareerPlan { get; set; } = null!;
        
        public Guid SkillId { get; set; }
        public Skill Skill { get; set; } = null!;
        
        public Guid? CareerStepId { get; set; }
        public CareerStep? CareerStep { get; set; }
        
        public SkillStatus Status { get; set; } = SkillStatus.Missing;
        public SkillLevel TargetLevel { get; set; } = SkillLevel.Intermediate;
    }
}