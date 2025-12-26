using CleanBackend.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Mentora.Domain.Entities
{
    public class LearningResource : BaseEntity
    {
        public Guid CareerStepId { get; set; }
        public CareerStep CareerStep { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string ResourceType { get; set; } = "Article";
        public bool IsOpened { get; set; }
    }
}