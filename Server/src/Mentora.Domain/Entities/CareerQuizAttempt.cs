namespace Mentora.Domain.Entities
{
    public class CareerQuizAttempt
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string AnswersJson { get; set; } = string.Empty; // JSON string of quiz answers
        public CareerQuizStatus Status { get; set; } = CareerQuizStatus.Draft;
        public DateTime SubmittedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public ICollection<CareerPlan> GeneratedPlans { get; set; } = new List<CareerPlan>();
    }
}
