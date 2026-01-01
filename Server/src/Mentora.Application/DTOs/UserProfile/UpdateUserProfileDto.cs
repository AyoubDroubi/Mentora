using System.ComponentModel.DataAnnotations;
using Mentora.Domain.Entities;

namespace Mentora.Application.DTOs.UserProfile
{
    /// <summary>
    /// DTO for creating/updating user profile per SRS Module 2
    /// Uses clean validation with proper error messages
    /// </summary>
    public class UpdateUserProfileDto
    {
        // Personal Information (Optional)
        [MaxLength(500, ErrorMessage = "Bio cannot exceed 500 characters")]
        public string? Bio { get; set; }

        [MaxLength(100, ErrorMessage = "Location cannot exceed 100 characters")]
        public string? Location { get; set; }

        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Phone number must be in international format (e.g., +962791234567)")]
        public string? PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        // Academic Attributes per SRS 2.1.1 (Required)
        [Required(ErrorMessage = "University name is required")]
        [MinLength(2, ErrorMessage = "University name must be at least 2 characters")]
        [MaxLength(200, ErrorMessage = "University name cannot exceed 200 characters")]
        public string University { get; set; } = string.Empty;

        [Required(ErrorMessage = "Major is required")]
        [MinLength(2, ErrorMessage = "Major must be at least 2 characters")]
        [MaxLength(200, ErrorMessage = "Major cannot exceed 200 characters")]
        public string Major { get; set; } = string.Empty;

        [Required(ErrorMessage = "Expected graduation year is required")]
        [Range(2024, 2050, ErrorMessage = "Graduation year must be between 2024 and 2050")]
        public int ExpectedGraduationYear { get; set; }

        // Study Level per SRS 2.1.2 (Required)
        [Required(ErrorMessage = "Study level is required")]
        [Range(0, 4, ErrorMessage = "Study level must be between 0 (Freshman) and 4 (Graduate)")]
        public int CurrentLevel { get; set; }

        // Timezone per SRS 2.2.1 (Required)
        [Required(ErrorMessage = "Timezone is required")]
        [MinLength(3, ErrorMessage = "Timezone must be at least 3 characters")]
        [MaxLength(50, ErrorMessage = "Timezone cannot exceed 50 characters")]
        public string Timezone { get; set; } = "UTC";

        // Social Links (Optional with URL validation)
        [Url(ErrorMessage = "LinkedIn URL must be a valid URL")]
        [MaxLength(500, ErrorMessage = "LinkedIn URL cannot exceed 500 characters")]
        public string? LinkedInUrl { get; set; }

        [Url(ErrorMessage = "GitHub URL must be a valid URL")]
        [MaxLength(500, ErrorMessage = "GitHub URL cannot exceed 500 characters")]
        public string? GitHubUrl { get; set; }

        [Url(ErrorMessage = "Avatar URL must be a valid URL")]
        [MaxLength(500, ErrorMessage = "Avatar URL cannot exceed 500 characters")]
        public string? AvatarUrl { get; set; }

        /// <summary>
        /// Get the StudyLevel enum from the integer value
        /// </summary>
        public StudyLevel GetStudyLevel() => (StudyLevel)CurrentLevel;
    }
}
