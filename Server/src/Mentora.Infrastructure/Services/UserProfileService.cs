using Mentora.Application.DTOs.UserProfile;
using Mentora.Application.Interfaces;
using Mentora.Domain.Entities;
using Mentora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Infrastructure.Services
{
    /// <summary>
    /// User Profile service implementation per SRS Module 2
    /// Implements academic attributes management (2.1) and system configurations (2.2)
    /// </summary>
    public class UserProfileService : IUserProfileService
    {
        private readonly ApplicationDbContext _context;

        // Common IANA timezones
        private static readonly List<string> CommonTimezones = new()
        {
            "UTC",
            "America/New_York",
            "America/Chicago",
            "America/Denver",
            "America/Los_Angeles",
            "Europe/London",
            "Europe/Paris",
            "Europe/Berlin",
            "Asia/Dubai",
            "Asia/Amman",
            "Asia/Riyadh",
            "Asia/Jerusalem",
            "Asia/Tokyo",
            "Asia/Shanghai",
            "Australia/Sydney",
            "Pacific/Auckland"
        };

        public UserProfileService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get user profile by user ID
        /// </summary>
        public async Task<UserProfileResponseDto?> GetProfileAsync(Guid userId)
        {
            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null)
                return null;

            return MapToDto(profile);
        }

        /// <summary>
        /// Create or update user profile per SRS 2.1.1, 2.1.2, 2.2.1
        /// Persists academic attributes and timezone configuration
        /// </summary>
        public async Task<UserProfileResponseDto> CreateOrUpdateProfileAsync(Guid userId, UpdateUserProfileDto dto)
        {
            // Validate timezone format per SRS 2.2.1
            if (!IsValidTimezone(dto.Timezone))
            {
                throw new ArgumentException("Invalid timezone format. Use IANA format (e.g., Asia/Amman)");
            }

            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null)
            {
                // Create new profile
                profile = new Domain.Entities.UserProfile
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };
                await _context.UserProfiles.AddAsync(profile);
            }

            // Update fields
            profile.Bio = dto.Bio ?? profile.Bio;
            profile.Location = dto.Location ?? profile.Location;
            profile.PhoneNumber = dto.PhoneNumber ?? profile.PhoneNumber;
            profile.DateOfBirth = dto.DateOfBirth ?? profile.DateOfBirth;

            // Academic Attributes per SRS 2.1.1
            profile.University = dto.University;
            profile.Major = dto.Major;
            profile.ExpectedGraduationYear = dto.ExpectedGraduationYear;

            // Study Level per SRS 2.1.2
            profile.CurrentLevel = dto.CurrentLevel;

            // Timezone per SRS 2.2.1
            profile.Timezone = dto.Timezone;

            // Social Links
            profile.LinkedInUrl = dto.LinkedInUrl ?? profile.LinkedInUrl;
            profile.GitHubUrl = dto.GitHubUrl ?? profile.GitHubUrl;
            profile.AvatarUrl = dto.AvatarUrl ?? profile.AvatarUrl;

            profile.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapToDto(profile);
        }

        /// <summary>
        /// Check if user has completed their profile
        /// </summary>
        public async Task<bool> HasProfileAsync(Guid userId)
        {
            return await _context.UserProfiles
                .AnyAsync(p => p.UserId == userId);
        }

        /// <summary>
        /// Get profile completion percentage
        /// </summary>
        public async Task<int> GetProfileCompletionAsync(Guid userId)
        {
            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null)
                return 0;

            int totalFields = 10;
            int completedFields = 0;

            // Required fields
            if (!string.IsNullOrEmpty(profile.University)) completedFields++;
            if (!string.IsNullOrEmpty(profile.Major)) completedFields++;
            if (profile.ExpectedGraduationYear > 0) completedFields++;
            if (!string.IsNullOrEmpty(profile.Timezone) && profile.Timezone != "UTC") completedFields++;

            // Optional but valuable fields
            if (!string.IsNullOrEmpty(profile.Bio)) completedFields++;
            if (!string.IsNullOrEmpty(profile.Location)) completedFields++;
            if (!string.IsNullOrEmpty(profile.PhoneNumber)) completedFields++;
            if (profile.DateOfBirth.HasValue) completedFields++;
            if (!string.IsNullOrEmpty(profile.LinkedInUrl)) completedFields++;
            if (!string.IsNullOrEmpty(profile.GitHubUrl)) completedFields++;

            return (int)((completedFields / (double)totalFields) * 100);
        }

        /// <summary>
        /// Validate timezone format (IANA) per SRS 2.2.1
        /// </summary>
        public bool IsValidTimezone(string timezone)
        {
            if (string.IsNullOrWhiteSpace(timezone))
                return false;

            try
            {
                // Try to find the timezone
                var tz = TimeZoneInfo.FindSystemTimeZoneById(timezone);
                return true;
            }
            catch
            {
                // Check if it's in our common list
                return CommonTimezones.Contains(timezone, StringComparer.OrdinalIgnoreCase);
            }
        }

        /// <summary>
        /// Get suggested timezones based on location
        /// </summary>
        public async Task<List<string>> GetSuggestedTimezonesAsync(string? location = null)
        {
            // If location is provided, try to suggest relevant timezones
            if (!string.IsNullOrEmpty(location))
            {
                var lowerLocation = location.ToLower();

                if (lowerLocation.Contains("jordan") || lowerLocation.Contains("amman"))
                    return new List<string> { "Asia/Amman", "Asia/Jerusalem", "Asia/Dubai" };

                if (lowerLocation.Contains("saudi") || lowerLocation.Contains("riyadh"))
                    return new List<string> { "Asia/Riyadh", "Asia/Dubai", "Asia/Kuwait" };

                if (lowerLocation.Contains("uae") || lowerLocation.Contains("dubai"))
                    return new List<string> { "Asia/Dubai", "Asia/Riyadh" };

                if (lowerLocation.Contains("egypt") || lowerLocation.Contains("cairo"))
                    return new List<string> { "Africa/Cairo", "Asia/Jerusalem" };

                if (lowerLocation.Contains("us") || lowerLocation.Contains("america"))
                    return new List<string> { "America/New_York", "America/Chicago", "America/Los_Angeles" };

                if (lowerLocation.Contains("uk") || lowerLocation.Contains("london"))
                    return new List<string> { "Europe/London", "Europe/Paris" };
            }

            // Return common timezones
            return CommonTimezones;
        }

        /// <summary>
        /// Map UserProfile entity to DTO
        /// </summary>
        private UserProfileResponseDto MapToDto(Domain.Entities.UserProfile profile)
        {
            var currentYear = DateTime.UtcNow.Year;
            var yearsUntilGraduation = profile.ExpectedGraduationYear - currentYear;

            int? age = null;
            if (profile.DateOfBirth.HasValue)
            {
                var today = DateTime.Today;
                age = today.Year - profile.DateOfBirth.Value.Year;
                if (profile.DateOfBirth.Value.Date > today.AddYears(-age.Value))
                    age--;
            }

            return new UserProfileResponseDto
            {
                Id = profile.Id,
                UserId = profile.UserId,
                Bio = profile.Bio,
                Location = profile.Location,
                PhoneNumber = profile.PhoneNumber,
                DateOfBirth = profile.DateOfBirth,
                University = profile.University,
                Major = profile.Major,
                ExpectedGraduationYear = profile.ExpectedGraduationYear,
                CurrentLevel = profile.CurrentLevel,
                CurrentLevelName = profile.CurrentLevel.ToString(),
                Timezone = profile.Timezone,
                LinkedInUrl = profile.LinkedInUrl,
                GitHubUrl = profile.GitHubUrl,
                AvatarUrl = profile.AvatarUrl,
                CreatedAt = profile.CreatedAt,
                UpdatedAt = profile.UpdatedAt,
                Age = age,
                YearsUntilGraduation = yearsUntilGraduation
            };
        }
    }
}
