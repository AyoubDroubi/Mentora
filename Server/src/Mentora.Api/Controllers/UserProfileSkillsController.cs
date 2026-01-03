using Mentora.Application.DTOs.UserProfile;
using Mentora.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Mentora.Api.Controllers
{
    /// <summary>
    /// Controller for User Profile Skills Management
    /// Per SRS 2.3: Skills Portfolio Management
    /// </summary>
    [ApiController]
    [Route("api/userprofile/skills")]
    [Authorize]
    public class UserProfileSkillsController : ControllerBase
    {
        private readonly IUserProfileSkillService _skillService;
        private readonly ILogger<UserProfileSkillsController> _logger;

        public UserProfileSkillsController(
            IUserProfileSkillService skillService,
            ILogger<UserProfileSkillsController> logger)
        {
            _skillService = skillService;
            _logger = logger;
        }

        /// <summary>
        /// Get all skills for current user with optional filtering
        /// GET /api/userprofile/skills?proficiencyLevel=2&isFeatured=true&sortBy=proficiency
        /// Per SRS 2.3.2.2: Retrieve Skills
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserProfileSkillDto>), 200)]
        public async Task<IActionResult> GetSkills(
            [FromQuery] int? proficiencyLevel = null,
            [FromQuery] bool? isFeatured = null,
            [FromQuery] string? sortBy = null)
        {
            try
            {
                var userId = GetUserId();
                var skills = await _skillService.GetUserSkillsAsync(
                    userId, proficiencyLevel, isFeatured, sortBy);

                return Ok(new
                {
                    success = true,
                    data = skills
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user skills");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while retrieving skills"
                });
            }
        }

        /// <summary>
        /// Get featured skills sorted by display order
        /// GET /api/userprofile/skills/featured
        /// Per SRS 2.3.6.3: Featured Retrieval
        /// </summary>
        [HttpGet("featured")]
        [ProducesResponseType(typeof(IEnumerable<UserProfileSkillDto>), 200)]
        public async Task<IActionResult> GetFeaturedSkills()
        {
            try
            {
                var userId = GetUserId();
                var skills = await _skillService.GetFeaturedSkillsAsync(userId);

                return Ok(new
                {
                    success = true,
                    data = skills
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting featured skills");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred"
                });
            }
        }

        /// <summary>
        /// Add a single skill to user profile
        /// POST /api/userprofile/skills
        /// Per SRS 2.3.2.1: Add Skills
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(UserProfileSkillDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> AddSkill([FromBody] AddSkillDto dto)
        {
            try
            {
                var userId = GetUserId();
                var skill = await _skillService.AddSkillAsync(userId, dto);

                return CreatedAtAction(
                    nameof(GetSkills),
                    new { id = skill.Id },
                    new
                    {
                        success = true,
                        message = "Skill added successfully",
                        data = skill
                    });
            }
            catch (InvalidOperationException ex)
            {
                // Business rule violations (max skills, duplicates, max featured)
                return Conflict(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding skill");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred"
                });
            }
        }

        /// <summary>
        /// Add multiple skills in bulk
        /// POST /api/userprofile/skills/bulk
        /// Per SRS 2.3.2.1: Add Bulk Skills
        /// </summary>
        [HttpPost("bulk")]
        [ProducesResponseType(typeof(BulkOperationResultDto), 200)]
        public async Task<IActionResult> AddSkillsBulk([FromBody] AddBulkSkillsDto dto)
        {
            try
            {
                var userId = GetUserId();
                var result = await _skillService.AddSkillsAsync(userId, dto);

                return Ok(new
                {
                    success = result.Success,
                    message = $"Added {result.SuccessCount} skills, {result.FailedCount} failed",
                    data = result
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding skills in bulk");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred"
                });
            }
        }

        /// <summary>
        /// Update a skill (partial update)
        /// PATCH /api/userprofile/skills/{id}
        /// Per SRS 2.3.2.3: Update Skills
        /// </summary>
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(UserProfileSkillDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateSkill(Guid id, [FromBody] UpdateSkillDto dto)
        {
            try
            {
                var userId = GetUserId();
                var skill = await _skillService.UpdateSkillAsync(userId, id, dto);

                return Ok(new
                {
                    success = true,
                    message = "Skill updated successfully",
                    data = skill
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating skill");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred"
                });
            }
        }

        /// <summary>
        /// Delete a single skill
        /// DELETE /api/userprofile/skills/{id}
        /// Per SRS 2.3.2.4: Delete Skills
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteSkill(Guid id)
        {
            try
            {
                var userId = GetUserId();
                var deleted = await _skillService.DeleteSkillAsync(userId, id);

                if (!deleted)
                    return NotFound(new
                    {
                        success = false,
                        message = "Skill not found"
                    });

                return Ok(new
                {
                    success = true,
                    message = "Skill deleted successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting skill");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred"
                });
            }
        }

        /// <summary>
        /// Delete multiple skills in bulk
        /// DELETE /api/userprofile/skills/bulk
        /// Per SRS 2.3.2.4: Delete Bulk Skills
        /// </summary>
        [HttpDelete("bulk")]
        [ProducesResponseType(typeof(BulkOperationResultDto), 200)]
        public async Task<IActionResult> DeleteSkillsBulk([FromBody] DeleteBulkSkillsDto dto)
        {
            try
            {
                var userId = GetUserId();
                var result = await _skillService.DeleteSkillsAsync(userId, dto);

                return Ok(new
                {
                    success = result.Success,
                    message = $"Deleted {result.SuccessCount} skills, {result.FailedCount} failed",
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting skills in bulk");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred"
                });
            }
        }

        /// <summary>
        /// Toggle featured status of a skill
        /// PATCH /api/userprofile/skills/{id}/featured
        /// Per SRS 2.3.6.2: Featured Toggle
        /// </summary>
        [HttpPatch("{id}/featured")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> ToggleFeatured(Guid id)
        {
            try
            {
                var userId = GetUserId();
                var success = await _skillService.ToggleFeaturedAsync(userId, id);

                if (!success)
                    return NotFound(new
                    {
                        success = false,
                        message = "Skill not found"
                    });

                return Ok(new
                {
                    success = true,
                    message = "Featured status updated successfully"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling featured status");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred"
                });
            }
        }

        /// <summary>
        /// Reorder featured skills
        /// PATCH /api/userprofile/skills/reorder
        /// Per SRS 2.3.6.1: Display Order Management
        /// </summary>
        [HttpPatch("reorder")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> ReorderSkills([FromBody] ReorderSkillsDto dto)
        {
            try
            {
                var userId = GetUserId();
                await _skillService.ReorderSkillsAsync(userId, dto);

                return Ok(new
                {
                    success = true,
                    message = "Skills reordered successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reordering skills");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred"
                });
            }
        }

        /// <summary>
        /// Get skills summary statistics
        /// GET /api/userprofile/skills/summary
        /// Per SRS 2.3.3.1: Skills Summary
        /// </summary>
        [HttpGet("summary")]
        [ProducesResponseType(typeof(SkillsSummaryDto), 200)]
        public async Task<IActionResult> GetSummary()
        {
            try
            {
                var userId = GetUserId();
                var summary = await _skillService.GetSkillsSummaryAsync(userId);

                return Ok(new
                {
                    success = true,
                    data = summary
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting skills summary");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred"
                });
            }
        }

        /// <summary>
        /// Get skills distribution analytics
        /// GET /api/userprofile/skills/distribution
        /// Per SRS 2.3.3.2: Skills Distribution
        /// </summary>
        [HttpGet("distribution")]
        [ProducesResponseType(typeof(SkillsDistributionDto), 200)]
        public async Task<IActionResult> GetDistribution()
        {
            try
            {
                var userId = GetUserId();
                var distribution = await _skillService.GetSkillsDistributionAsync(userId);

                return Ok(new
                {
                    success = true,
                    data = distribution
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting skills distribution");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred"
                });
            }
        }

        /// <summary>
        /// Get skills timeline
        /// GET /api/userprofile/skills/timeline
        /// Per SRS 2.3.3.3: Skills Timeline
        /// </summary>
        [HttpGet("timeline")]
        [ProducesResponseType(typeof(SkillsTimelineDto), 200)]
        public async Task<IActionResult> GetTimeline()
        {
            try
            {
                var userId = GetUserId();
                var timeline = await _skillService.GetSkillsTimelineAsync(userId);

                return Ok(new
                {
                    success = true,
                    data = timeline
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting skills timeline");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred"
                });
            }
        }

        /// <summary>
        /// Get skills coverage analysis
        /// GET /api/userprofile/skills/coverage
        /// Per SRS 2.3.3.4: Skills Coverage Analysis
        /// </summary>
        [HttpGet("coverage")]
        [ProducesResponseType(typeof(SkillsCoverageDto), 200)]
        public async Task<IActionResult> GetCoverage()
        {
            try
            {
                var userId = GetUserId();
                var coverage = await _skillService.GetSkillsCoverageAsync(userId);

                return Ok(new
                {
                    success = true,
                    data = coverage
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting skills coverage");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred"
                });
            }
        }

        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedAccessException("User ID not found in token");

            return userId;
        }
    }
}
