using Mentora.Domain.Common;

namespace Mentora.Domain.Entities
{
    public class Skill : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public SkillCategory Category { get; set; } = SkillCategory.Technical;
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