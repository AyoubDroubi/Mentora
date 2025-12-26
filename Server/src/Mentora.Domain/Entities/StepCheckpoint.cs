using Mentora.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Mentora.Domain.Entities
{
    public class StepCheckpoint : BaseEntity
    {
        public Guid CareerStepId { get; set; }
        public CareerStep CareerStep { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
        public int OrderIndex { get; set; }
    }
}