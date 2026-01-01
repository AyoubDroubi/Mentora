using Mentora.Domain.Entities;

namespace Mentora.Application.DTOs.UserProfile
{
    /// <summary>
    /// Response DTO for user profile data
    /// </summary>
    public class UserProfileResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        
        // Personal Information
        public string Bio { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        
        // Academic Attributes per SRS 2.1.1
        public string University { get; set; } = string.Empty;
        public string Major { get; set; } = string.Empty;
        public int ExpectedGraduationYear { get; set; }
        
        // Study Level per SRS 2.1.2
        public StudyLevel CurrentLevel { get; set; }
        public string CurrentLevelName { get; set; } = string.Empty;
        
        // Timezone per SRS 2.2.1
        public string Timezone { get; set; } = "UTC";
        
        // Social Links
        public string LinkedInUrl { get; set; } = string.Empty;
        public string GitHubUrl { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        
        // Metadata
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Computed Properties
        public int? Age { get; set; }
        public int YearsUntilGraduation { get; set; }
    }
}
