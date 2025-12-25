using System.ComponentModel.DataAnnotations;

namespace Mentora.Domain.Entities
{
    public class StepCheckpoint
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CareerStepId { get; set; }
        public CareerStep CareerStep { get; set; } = null!;

        public string Title { get; set; } = string.Empty; // "Install Node.js"
        public bool IsCompleted { get; set; } = false;

        // ترتيبه داخل الخطوة
        public int OrderIndex { get; set; }
    }
}