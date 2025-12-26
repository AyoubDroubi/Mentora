using CleanBackend.Domain.Common;
using CleanBackend.Domain.Entities.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentora.Domain.Entities
{
    public class UserProfile : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public string University { get; set; } = string.Empty;
        public string Major { get; set; } = string.Empty;
        public StudyLevel CurrentLevel { get; set; } = StudyLevel.Freshman;
        public int ExpectedGraduationYear { get; set; }
        public string Bio { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
    }
}