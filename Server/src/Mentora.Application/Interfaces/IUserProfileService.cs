using Mentora.Application.DTOs.UserProfile;

namespace Mentora.Application.Interfaces
{
    /// <summary>
    /// User Profile service interface per SRS Module 2
    /// Handles academic attributes management and system configurations
    /// </summary>
    public interface IUserProfileService
    {
        /// <summary>
        /// Get user profile by user ID
        /// </summary>
        Task<UserProfileResponseDto?> GetProfileAsync(Guid userId);

        /// <summary>
        /// Create or update user profile
        /// Per SRS 2.1.1, 2.1.2, 2.2.1
        /// </summary>
        Task<UserProfileResponseDto> CreateOrUpdateProfileAsync(Guid userId, UpdateUserProfileDto dto);

        /// <summary>
        /// Check if user has completed their profile
        /// </summary>
        Task<bool> HasProfileAsync(Guid userId);

        /// <summary>
        /// Get profile completion percentage
        /// </summary>
        Task<int> GetProfileCompletionAsync(Guid userId);

        /// <summary>
        /// Validate timezone format (IANA)
        /// Per SRS 2.2.1
        /// </summary>
        bool IsValidTimezone(string timezone);

        /// <summary>
        /// Get suggested timezones based on location
        /// </summary>
        Task<List<string>> GetSuggestedTimezonesAsync(string? location = null);
    }
}
