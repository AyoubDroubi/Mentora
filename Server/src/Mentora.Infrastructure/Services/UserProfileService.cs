using Mentora.Application.DTOs.UserProfile;
using Mentora.Application.Interfaces;
using Mentora.Domain.Entities;
using Mentora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Mentora.Infrastructure.Services
{
    /// <summary>
    /// User Profile service implementation per SRS Module 2
    /// Clean, professional implementation with proper error handling
    /// </summary>
    public class UserProfileService : IUserProfileService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserProfileService> _logger;

        // IANA timezones organized by region
        private static readonly List<string> CommonTimezones = new()
        {
            "UTC",
            // Middle East
            "Asia/Amman", "Asia/Jerusalem", "Asia/Dubai", "Asia/Riyadh", 
            "Asia/Kuwait", "Asia/Baghdad", "Asia/Beirut",
            // North Africa
            "Africa/Cairo", "Africa/Casablanca", "Africa/Tunis",
            // Europe
            "Europe/London", "Europe/Paris", "Europe/Berlin", "Europe/Rome", "Europe/Istanbul",
            // Americas
            "America/New_York", "America/Chicago", "America/Denver", "America/Los_Angeles",
            "America/Toronto", "America/Mexico_City",
            // Asia Pacific
            "Asia/Tokyo", "Asia/Shanghai", "Asia/Singapore", "Asia/Hong_Kong",
            "Australia/Sydney", "Pacific/Auckland"
        };

        public UserProfileService(ApplicationDbContext context, ILogger<UserProfileService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<UserProfileResponseDto?> GetProfileAsync(Guid userId)
        {
            try
            {
                var profile = await _context.UserProfiles
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.UserId == userId);

                return profile == null ? null : MapToDto(profile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving profile for user {UserId}", userId);
                throw;
            }
        }

        public async Task<UserProfileResponseDto> CreateOrUpdateProfileAsync(Guid userId, UpdateUserProfileDto dto)
        {
            try
            {
                // Validate timezone
                if (!IsValidTimezone(dto.Timezone))
                {
                    throw new ArgumentException($"Invalid timezone '{dto.Timezone}'. Use IANA format (e.g., Asia/Amman) or UTC.");
                }

                var profile = await _context.UserProfiles
                    .FirstOrDefaultAsync(p => p.UserId == userId);

                var isNew = profile == null;

                if (isNew)
                {
                    profile = new Domain.Entities.UserProfile
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.UserProfiles.Add(profile);
                }

                // Update profile fields
                UpdateProfileFields(profile, dto);
                profile.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "{Action} profile for user {UserId}",
                    isNew ? "Created" : "Updated",
                    userId
                );

                return MapToDto(profile);
            }
            catch (ArgumentException)
            {
                throw; // Re-throw validation errors
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating/updating profile for user {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> HasProfileAsync(Guid userId)
        {
            return await _context.UserProfiles.AnyAsync(p => p.UserId == userId);
        }

        public async Task<int> GetProfileCompletionAsync(Guid userId)
        {
            var profile = await _context.UserProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null) return 0;

            var fields = new[]
            {
                !string.IsNullOrWhiteSpace(profile.University),
                !string.IsNullOrWhiteSpace(profile.Major),
                profile.ExpectedGraduationYear >= DateTime.UtcNow.Year,
                !string.IsNullOrWhiteSpace(profile.Timezone) && profile.Timezone != "UTC",
                !string.IsNullOrWhiteSpace(profile.Bio),
                !string.IsNullOrWhiteSpace(profile.Location),
                !string.IsNullOrWhiteSpace(profile.PhoneNumber),
                profile.DateOfBirth.HasValue,
                !string.IsNullOrWhiteSpace(profile.LinkedInUrl),
                !string.IsNullOrWhiteSpace(profile.GitHubUrl)
            };

            var completed = fields.Count(f => f);
            return (int)Math.Round((completed / (double)fields.Length) * 100);
        }

        public bool IsValidTimezone(string timezone)
        {
            if (string.IsNullOrWhiteSpace(timezone)) return false;

            try
            {
                TimeZoneInfo.FindSystemTimeZoneById(timezone);
                return true;
            }
            catch
            {
                return CommonTimezones.Contains(timezone, StringComparer.OrdinalIgnoreCase);
            }
        }

        public Task<List<string>> GetSuggestedTimezonesAsync(string? location = null)
        {
            if (string.IsNullOrWhiteSpace(location))
                return Task.FromResult(CommonTimezones);

            var lowerLocation = location.ToLower();
            var suggestions = lowerLocation switch
            {
                var l when l.Contains("jordan") || l.Contains("amman") => 
                    new List<string> { "Asia/Amman", "Asia/Jerusalem", "Asia/Dubai" },
                var l when l.Contains("saudi") || l.Contains("riyadh") => 
                    new List<string> { "Asia/Riyadh", "Asia/Dubai", "Asia/Kuwait" },
                var l when l.Contains("uae") || l.Contains("dubai") => 
                    new List<string> { "Asia/Dubai", "Asia/Riyadh", "Asia/Kuwait" },
                var l when l.Contains("egypt") || l.Contains("cairo") => 
                    new List<string> { "Africa/Cairo", "Africa/Tunis" },
                var l when l.Contains("palestine") || l.Contains("gaza") => 
                    new List<string> { "Asia/Jerusalem", "Asia/Amman" },
                var l when l.Contains("us") || l.Contains("america") => 
                    new List<string> { "America/New_York", "America/Chicago", "America/Los_Angeles" },
                var l when l.Contains("uk") || l.Contains("london") || l.Contains("britain") => 
                    new List<string> { "Europe/London", "Europe/Paris", "Europe/Dublin" },
                _ => CommonTimezones
            };

            return Task.FromResult(suggestions);
        }

        private void UpdateProfileFields(Domain.Entities.UserProfile profile, UpdateUserProfileDto dto)
        {
            // Personal information
            profile.Bio = dto.Bio ?? string.Empty;
            profile.Location = dto.Location ?? string.Empty;
            profile.PhoneNumber = dto.PhoneNumber ?? string.Empty;
            profile.DateOfBirth = dto.DateOfBirth;

            // Academic attributes (required)
            profile.University = dto.University.Trim();
            profile.Major = dto.Major.Trim();
            profile.ExpectedGraduationYear = dto.ExpectedGraduationYear;
            profile.CurrentLevel = dto.GetStudyLevel();

            // System configuration
            profile.Timezone = dto.Timezone.Trim();

            // Social links
            profile.LinkedInUrl = dto.LinkedInUrl ?? string.Empty;
            profile.GitHubUrl = dto.GitHubUrl ?? string.Empty;
            profile.AvatarUrl = dto.AvatarUrl ?? string.Empty;
        }

        private UserProfileResponseDto MapToDto(Domain.Entities.UserProfile profile)
        {
            var currentYear = DateTime.UtcNow.Year;
            var yearsUntilGraduation = Math.Max(0, profile.ExpectedGraduationYear - currentYear);

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
