using Mentora.Domain.Common;

namespace Mentora.Domain.Entities
{
    public class Skill : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
    }

    public class CareerPlanSkill : BaseEntity
    {
        public Guid CareerPlanId { get; set; }
        public CareerPlan CareerPlan { get; set; } = null!;
        public Guid SkillId { get; set; }
        public Skill Skill { get; set; } = null!;
        public SkillLevel TargetLevel { get; set; } = SkillLevel.Beginner;
    }
}