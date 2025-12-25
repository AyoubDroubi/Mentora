using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentora.Domain.Entities
{
    public class UserProfile
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!; 

        [MaxLength(200)]
        public string University { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Major { get; set; } = string.Empty;

        public StudyLevel CurrentLevel { get; set; } = StudyLevel.Freshman;

        public int ExpectedGraduationYear { get; set; }

        [MaxLength(500)]
        public string Bio { get; set; } = string.Empty;

        public string AvatarUrl { get; set; } = string.Empty;
    }
}
