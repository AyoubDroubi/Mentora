using System.ComponentModel.DataAnnotations;
using Mentora.Domain.Entities;

namespace Mentora.Application.DTOs.UserProfile
{
    /// <summary>
    /// DTO for creating/updating user profile per SRS Module 2
    /// </summary>
    public class UpdateUserProfileDto
    {
        [MaxLength(500, ErrorMessage = "Bio cannot exceed 500 characters")]
        public string? Bio { get; set; }

        [MaxLength(100, ErrorMessage = "Location cannot exceed 100 characters")]
        public string? Location { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        public string? PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        // Academic Attributes per SRS 2.1.1
        [Required(ErrorMessage = "University is required")]
        [MaxLength(200, ErrorMessage = "University name cannot exceed 200 characters")]
        public string University { get; set; } = string.Empty;

        [Required(ErrorMessage = "Major is required")]
        [MaxLength(200, ErrorMessage = "Major cannot exceed 200 characters")]
        public string Major { get; set; } = string.Empty;

        [Required(ErrorMessage = "Expected graduation year is required")]
        [Range(2024, 2050, ErrorMessage = "Graduation year must be between 2024 and 2050")]
        public int ExpectedGraduationYear { get; set; }

        // Study Level per SRS 2.1.2
        [Required(ErrorMessage = "Study level is required")]
        public StudyLevel CurrentLevel { get; set; }

        // Timezone per SRS 2.2.1
        // Accept UTC or IANA format (e.g., Asia/Amman, America/New_York)
        [Required(ErrorMessage = "Timezone is required")]
        [RegularExpression(@"^(UTC|[A-Za-z_]+/[A-Za-z_]+)$", 
            ErrorMessage = "Timezone must be UTC or in IANA format (e.g., Asia/Amman)")]
        public string Timezone { get; set; } = "UTC";

        // Social Links
        [Url(ErrorMessage = "Invalid LinkedIn URL format")]
        public string? LinkedInUrl { get; set; }

        [Url(ErrorMessage = "Invalid GitHub URL format")]
        public string? GitHubUrl { get; set; }

        [Url(ErrorMessage = "Invalid avatar URL format")]
        public string? AvatarUrl { get; set; }
    }
}
