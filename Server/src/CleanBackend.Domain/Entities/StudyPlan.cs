namespace Mentora.Domain.Entities
{
    public class StudyPlan
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; } // FK User
        public User User { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string StrategyName { get; set; } = string.Empty; // e.g., "Exam Cram"
        public bool IsActive { get; set; } = true;
    }
}