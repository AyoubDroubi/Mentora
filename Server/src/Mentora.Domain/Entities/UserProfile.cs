using Mentora.Domain.Common;
using Mentora.Domain.Entities.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentora.Domain.Entities
{
    public class UserProfile : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public string Bio { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string University { get; set; } = string.Empty;
        public string Major { get; set; } = string.Empty;
        public int? GraduationYear { get; set; }
        public string LinkedInUrl { get; set; } = string.Empty;
        public string GitHubUrl { get; set; } = string.Empty;
        public StudyLevel CurrentLevel { get; set; } = StudyLevel.Freshman;
        public int ExpectedGraduationYear { get; set; }
        public string AvatarUrl { get; set; } = string.Empty;
    }
}