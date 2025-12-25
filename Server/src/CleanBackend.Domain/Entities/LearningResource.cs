using System.ComponentModel.DataAnnotations;

namespace Mentora.Domain.Entities
{
    public class LearningResource
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CareerStepId { get; set; }
        public CareerStep CareerStep { get; set; } = null!;

        public string Title { get; set; } = string.Empty; // "React Docs"
        public string Url { get; set; } = string.Empty;

        // Type: "Video", "Article", "Book", "Course"
        public string ResourceType { get; set; } = "Article";

        public bool IsOpened { get; set; } // هل الطالب فتح الرابط؟
    }
}