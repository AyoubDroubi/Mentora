namespace Mentora.Domain.Entities
{
    public class Skill
    {
        public int Id { get; set; } // int كافي
        public string Name { get; set; } = string.Empty; // "Python", "Communication"
    }

    // الجدول الوسيط (Many-to-Many)
    public class CareerPlanSkill
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CareerPlanId { get; set; }
        public CareerPlan CareerPlan { get; set; } = null!;

        public int SkillId { get; set; }
        public Skill Skill { get; set; } = null!;

        // شو المستوى المطلوب لهاي المهارة بهاي الخطة؟
        public string TargetLevel { get; set; } = "Beginner"; // Beginner, Intermediate
    }
}