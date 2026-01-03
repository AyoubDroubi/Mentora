using Mentora.Domain.Common;
using Mentora.Domain.Entities.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentora.Domain.Entities
{
    /// <summary>
    /// User Profile entity per SRS Module 2: User Profile & Personalization
    /// Contains academic attributes (2.1.1) and system configurations (2.2.1)
    /// </summary>
    public class UserProfile : BaseEntity
    {
        // User Relationship
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        
        // Personal Information
        public string Bio { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        
        // Academic Attributes per SRS 2.1.1
        public string University { get; set; } = string.Empty;
        public string Major { get; set; } = string.Empty;
        public int ExpectedGraduationYear { get; set; }
        
        // Study Level Logic per SRS 2.1.2
        public StudyLevel CurrentLevel { get; set; } = StudyLevel.Freshman;
        
        // Timezone Synchronization per SRS 2.2.1
        /// <summary>
        /// User timezone in IANA format (e.g., "Asia/Amman", "America/New_York")
        /// Used to align notifications and task schedules with user's local time
        /// </summary>
        public string Timezone { get; set; } = "UTC";
        
        // Legacy field (optional, can be removed if not used)
        public int? GraduationYear { get; set; }
        
        // Social Links
        public string LinkedInUrl { get; set; } = string.Empty;
        public string GitHubUrl { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        
        // Navigation Properties
        /// <summary>
        /// User Profile Skills - Many-to-Many relationship with Skill entity
        /// Per SRS 2.3.1: Skills Portfolio Management
        /// </summary>
        public ICollection<UserProfileSkill> Skills { get; set; } = new List<UserProfileSkill>();
    }
}