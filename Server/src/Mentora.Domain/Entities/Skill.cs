using Mentora.Domain.Common;

namespace Mentora.Domain.Entities
{
    public class Skill : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class CareerPlanSkill : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CareerPlanId { get; set; }
        public CareerPlan CareerPlan { get; set; } = null!;
        public int SkillId { get; set; }
        public Skill Skill { get; set; } = null!;
        public SkillLevel TargetLevel { get; set; } = SkillLevel.Beginner;
    }
}