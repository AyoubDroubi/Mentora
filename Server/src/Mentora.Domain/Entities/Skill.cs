using Mentora.Domain.Common;

namespace Mentora.Domain.Entities
{
    public class Skill : BaseEntity // حذفنا int Id لأن BaseEntity يعطيه Guid تلقائياً
    {
        public string Name { get; set; } = string.Empty;
    }

    public class CareerPlanSkill : BaseEntity
    {
        public Guid CareerPlanId { get; set; }
        public CareerPlan CareerPlan { get; set; } = null!;
        public Guid SkillId { get; set; } // تم التغيير من int لـ Guid
        public Skill Skill { get; set; } = null!;
        public SkillLevel TargetLevel { get; set; } = SkillLevel.Beginner;
    }
}