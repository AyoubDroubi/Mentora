using Mentora.Domain.Common;
using Mentora.Domain.Entities.Auth;
using System.ComponentModel.DataAnnotations;

namespace Mentora.Domain.Entities
{
    /// <summary>
    /// UserProfileSkill entity per SRS 2.3.1: User Profile Skills Entity
    /// Many-to-Many relationship between UserProfile and Skill
    /// </summary>
    public class UserProfileSkill : BaseEntity
    {
        // Foreign Keys
        public Guid UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; } = null!;
        
        public Guid SkillId { get; set; }
        public Skill Skill { get; set; } = null!;
        
        // Proficiency Level (SRS 2.3.1.1)
        /// <summary>
        /// Proficiency level: 0-3 (Beginner, Intermediate, Advanced, Expert)
        /// </summary>
        [Range(0, 3)]
        public int ProficiencyLevel { get; set; } = 0; // 0 = Beginner
        
        // Acquisition Details (SRS 2.3.1.1)
        /// <summary>
        /// How the skill was acquired (e.g., "Online Course", "University", "Self-taught")
        /// Max 200 characters
        /// </summary>
        [MaxLength(200)]
        public string? AcquisitionMethod { get; set; }
        
        /// <summary>
        /// When learning began (nullable)
        /// </summary>
        public DateTime? StartedDate { get; set; }
        
        /// <summary>
        /// Total years of experience with this skill (0-50 range)
        /// </summary>
        [Range(0, 50)]
        public int? YearsOfExperience { get; set; }
        
        // Featured & Display (SRS 2.3.1.1)
        /// <summary>
        /// Display on public profile
        /// Maximum 10 featured skills per profile (enforced in service layer)
        /// </summary>
        public bool IsFeatured { get; set; } = false;
        
        /// <summary>
        /// Ordering for featured skills showcase
        /// Used for custom sorting of featured skills
        /// </summary>
        public int DisplayOrder { get; set; } = 0;
        
        // Additional Notes (SRS 2.3.1.1)
        /// <summary>
        /// Personal notes about the skill
        /// Max 1000 characters
        /// </summary>
        [MaxLength(1000)]
        public string? Notes { get; set; }
    }
    
    /// <summary>
    /// Enum for Proficiency Levels (0-3)
    /// </summary>
    public enum SkillProficiencyLevel
    {
        Beginner = 0,
        Intermediate = 1,
        Advanced = 2,
        Expert = 3
    }

    /// <summary>
    /// Legacy UserSkill entity - Deprecated
    /// Use UserProfileSkill instead per SRS 2.3.1
    /// </summary>
    [Obsolete("Use UserProfileSkill instead")]
    public class UserSkill : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid SkillId { get; set; }
        public Skill Skill { get; set; } = null!;
        public SkillLevel CurrentLevel { get; set; } = SkillLevel.Beginner;
    }
}