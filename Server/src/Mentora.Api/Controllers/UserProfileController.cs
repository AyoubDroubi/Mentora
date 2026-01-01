using Mentora.Application.DTOs.UserProfile;
using Mentora.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mentora.Api.Controllers
{
    /// <summary>
    /// User Profile Management API - Professional implementation
    /// Implements SRS Module 2: User Profile & Personalization
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _profileService;
        private readonly ILogger<UserProfileController> _logger;

        public UserProfileController(IUserProfileService profileService, ILogger<UserProfileController> logger)
        {
            _profileService = profileService;
            _logger = logger;
        }

        /// <summary>
        /// Get current user's profile
        /// </summary>
        /// <returns>User profile data including academic attributes and timezone</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/userprofile
        ///     Authorization: Bearer {your-access-token}
        ///
        /// Returns profile with:
        /// - Academic attributes (University, Major, ExpectedGraduationYear) per SRS 2.1.1
        /// - Study level (Freshman to Graduate) per SRS 2.1.2
        /// - Timezone in IANA format per SRS 2.2.1
        /// </remarks>
        /// <response code="200">Profile retrieved successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Profile not found</response>
        [HttpGet]
        [ProducesResponseType(typeof(UserProfileResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProfile()
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
                return Unauthorized(new { message = "User not authenticated" });

            var profile = await _profileService.GetProfileAsync(userId);

            if (profile == null)
                return NotFound(new { message = "Profile not found. Please complete your profile." });

            return Ok(profile);
        }

        /// <summary>
        /// Create or update user profile
        /// </summary>
        /// <param name="dto">Profile data including academic attributes and timezone</param>
        /// <returns>Updated profile</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/userprofile
        ///     Authorization: Bearer {your-access-token}
        ///     {
        ///        "university": "University of Jordan",
        ///        "major": "Computer Science",
        ///        "expectedGraduationYear": 2026,
        ///        "currentLevel": "Junior",
        ///        "timezone": "Asia/Amman",
        ///        "bio": "Passionate CS student",
        ///        "location": "Amman, Jordan",
        ///        "phoneNumber": "+962791234567",
        ///        "linkedInUrl": "https://linkedin.com/in/username",
        ///        "gitHubUrl": "https://github.com/username"
        ///     }
        ///
        /// Academic Attributes (SRS 2.1.1):
        /// - University: Required, max 200 characters
        /// - Major: Required, max 200 characters
        /// - ExpectedGraduationYear: Required, 2024-2050
        ///
        /// Study Level (SRS 2.1.2):
        /// - CurrentLevel: Freshman, Sophomore, Junior, Senior, or Graduate (can be string or int)
        ///
        /// Timezone (SRS 2.2.1):
        /// - IANA format required (e.g., Asia/Amman, America/New_York) or UTC
        /// - Used for notification and schedule synchronization
        /// </remarks>
        /// <response code="200">Profile updated successfully</response>
        /// <response code="400">Invalid input data</response>
        /// <response code="401">Unauthorized</response>
        [HttpPut]
        [ProducesResponseType(typeof(UserProfileResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileDto dto)
        {
            // Check authentication
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
                return Unauthorized(new { message = "User not authenticated" });

            // Validate model
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                _logger.LogWarning(
                    "Profile update validation failed for user {UserId}. Errors: {@Errors}",
                    userId,
                    errors
                );

                return BadRequest(new
                {
                    message = "Validation failed",
                    errors = errors
                });
            }

            // Additional timezone validation
            if (!_profileService.IsValidTimezone(dto.Timezone))
            {
                return BadRequest(new
                {
                    message = "Validation failed",
                    errors = new Dictionary<string, string[]>
                    {
                        ["Timezone"] = new[] { $"Invalid timezone '{dto.Timezone}'. Use IANA format (e.g., Asia/Amman, UTC)." }
                    }
                });
            }

            try
            {
                var profile = await _profileService.CreateOrUpdateProfileAsync(userId, dto);
                _logger.LogInformation("Profile updated successfully for user {UserId}", userId);
                return Ok(profile);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error for user {UserId}", userId);
                return BadRequest(new
                {
                    message = "Validation error",
                    errors = new Dictionary<string, string[]>
                    {
                        ["Validation"] = new[] { ex.Message }
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating profile for user {UserId}", userId);
                return StatusCode(500, new
                {
                    message = "An unexpected error occurred while updating your profile. Please try again."
                });
            }
        }

        /// <summary>
        /// Check if user has a profile
        /// </summary>
        /// <returns>Boolean indicating profile existence</returns>
        /// <response code="200">Check completed</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("exists")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> HasProfile()
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
                return Unauthorized(new { message = "User not authenticated" });

            var exists = await _profileService.HasProfileAsync(userId);
            return Ok(new { exists });
        }

        /// <summary>
        /// Get profile completion percentage
        /// </summary>
        /// <returns>Completion percentage (0-100)</returns>
        /// <remarks>
        /// Calculates completion based on:
        /// - Required fields: University, Major, ExpectedGraduationYear, Timezone
        /// - Optional fields: Bio, Location, PhoneNumber, DateOfBirth, LinkedIn, GitHub
        /// </remarks>
        /// <response code="200">Completion percentage retrieved</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("completion")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCompletion()
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
                return Unauthorized(new { message = "User not authenticated" });

            var completion = await _profileService.GetProfileCompletionAsync(userId);
            return Ok(new { completionPercentage = completion });
        }

        /// <summary>
        /// Get suggested timezones based on location
        /// </summary>
        /// <param name="location">Optional location to get relevant suggestions</param>
        /// <returns>List of suggested IANA timezone identifiers</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/userprofile/timezones?location=Jordan
        ///
        /// Returns relevant timezones based on location.
        /// If no location provided, returns common timezones.
        /// </remarks>
        /// <response code="200">Timezones retrieved successfully</response>
        [HttpGet("timezones")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTimezones([FromQuery] string? location = null)
        {
            var timezones = await _profileService.GetSuggestedTimezonesAsync(location);
            return Ok(timezones);
        }

        /// <summary>
        /// Validate timezone format (IANA)
        /// </summary>
        /// <param name="timezone">Timezone to validate</param>
        /// <returns>Boolean indicating validity</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/userprofile/validate-timezone?timezone=Asia/Amman
        ///
        /// Validates IANA format per SRS 2.2.1
        /// </remarks>
        /// <response code="200">Validation result returned</response>
        [HttpGet("validate-timezone")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public IActionResult ValidateTimezone([FromQuery] string timezone)
        {
            if (string.IsNullOrWhiteSpace(timezone))
                return BadRequest(new { message = "Timezone parameter is required" });

            var isValid = _profileService.IsValidTimezone(timezone);
            return Ok(new { isValid, timezone });
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }
    }
}
