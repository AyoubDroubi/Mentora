using Mentora.Application.DTOs.Assessment;
using Mentora.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mentora.Api.Controllers
{
    /// <summary>
    /// Study Plan API endpoints per SRS Section 3 & 4
    /// Provides AI-generated study plan management and progress tracking
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudyPlanController : BaseController
    {
        private readonly IStudyPlanService _studyPlanService;
        private readonly ILogger<StudyPlanController> _logger;

        public StudyPlanController(
            IStudyPlanService studyPlanService,
            ILogger<StudyPlanController> logger)
        {
            _studyPlanService = studyPlanService;
            _logger = logger;
        }

        /// <summary>
        /// Generate AI study plan per SRS 3.2
        /// </summary>
        /// <param name="request">Study plan generation request</param>
        /// <returns>Generated study plan summary</returns>
        /// <response code="201">Study plan generated successfully</response>
        /// <response code="400">Invalid request</response>
        /// <response code="403">Access denied to assessment</response>
        /// <response code="500">AI service error</response>
        [HttpPost("generate")]
        [ProducesResponseType(typeof(GenerateStudyPlanResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GenerateStudyPlanResponseDto>> GenerateStudyPlan(
            [FromBody] GenerateStudyPlanRequestDto request)
        {
            try
            {
                var userId = GetUserId();
                _logger.LogInformation($"?? User {userId} requesting study plan generation");
                
                var result = await _studyPlanService.GenerateStudyPlanAsync(userId, request);
                
                return CreatedAtAction(
                    nameof(GetStudyPlan),
                    new { studyPlanId = result.StudyPlanId },
                    result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access to assessment");
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid study plan operation");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating study plan");
                return StatusCode(500, new { message = "AI service error. Please try again.", error = ex.Message });
            }
        }

        /// <summary>
        /// Get study plan details per SRS 3.2
        /// </summary>
        /// <param name="studyPlanId">Study plan ID</param>
        /// <param name="includeAll">Include all nested data (steps, resources, skills)</param>
        /// <returns>Study plan with details</returns>
        /// <response code="200">Study plan retrieved successfully</response>
        /// <response code="403">Access denied</response>
        /// <response code="404">Study plan not found</response>
        [HttpGet("{studyPlanId}")]
        [ProducesResponseType(typeof(StudyPlanDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudyPlanDto>> GetStudyPlan(
            Guid studyPlanId,
            [FromQuery] bool includeAll = true)
        {
            try
            {
                var userId = GetUserId();
                _logger.LogInformation($"?? User {userId} retrieving study plan {studyPlanId}");
                
                var studyPlan = await _studyPlanService.GetStudyPlanAsync(userId, studyPlanId, includeAll);
                return Ok(studyPlan);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access to study plan");
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving study plan");
                return StatusCode(500, new { message = "Error retrieving study plan", error = ex.Message });
            }
        }

        /// <summary>
        /// Get all study plans for current user
        /// </summary>
        /// <returns>List of user's study plans</returns>
        /// <response code="200">Study plans retrieved successfully</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<StudyPlanDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<StudyPlanDto>>> GetUserStudyPlans()
        {
            try
            {
                var userId = GetUserId();
                _logger.LogInformation($"?? User {userId} retrieving all study plans");
                
                var studyPlans = await _studyPlanService.GetUserStudyPlansAsync(userId);
                return Ok(studyPlans);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user study plans");
                return StatusCode(500, new { message = "Error retrieving study plans", error = ex.Message });
            }
        }

        /// <summary>
        /// Get active study plan for current user
        /// </summary>
        /// <returns>Active study plan or null</returns>
        /// <response code="200">Active study plan retrieved successfully</response>
        /// <response code="204">No active study plan</response>
        [HttpGet("active")]
        [ProducesResponseType(typeof(StudyPlanDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<StudyPlanDto>> GetActiveStudyPlan()
        {
            try
            {
                var userId = GetUserId();
                _logger.LogInformation($"?? User {userId} retrieving active study plan");
                
                var studyPlan = await _studyPlanService.GetActiveStudyPlanAsync(userId);
                
                if (studyPlan == null)
                    return NoContent();

                return Ok(studyPlan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active study plan");
                return StatusCode(500, new { message = "Error retrieving active study plan", error = ex.Message });
            }
        }

        /// <summary>
        /// Update study plan progress per SRS 5.2
        /// </summary>
        /// <param name="update">Progress update data</param>
        /// <returns>Success result</returns>
        /// <response code="200">Progress updated successfully</response>
        /// <response code="400">Invalid update data</response>
        /// <response code="403">Access denied</response>
        [HttpPatch("progress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> UpdateProgress([FromBody] UpdateStudyPlanProgressDto update)
        {
            try
            {
                var userId = GetUserId();
                _logger.LogInformation($"?? User {userId} updating progress for plan {update.StudyPlanId}");
                
                await _studyPlanService.UpdateProgressAsync(userId, update);
                return Ok(new { message = "Progress updated successfully" });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access to study plan");
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating progress");
                return StatusCode(500, new { message = "Error updating progress", error = ex.Message });
            }
        }

        /// <summary>
        /// Analyze skill gaps per SRS 3.3.3
        /// </summary>
        /// <param name="studyPlanId">Study plan ID</param>
        /// <returns>Skill gap analysis with recommendations</returns>
        /// <response code="200">Analysis completed successfully</response>
        /// <response code="403">Access denied</response>
        /// <response code="404">Study plan or user profile not found</response>
        [HttpGet("{studyPlanId}/skill-gaps")]
        [ProducesResponseType(typeof(SkillGapAnalysisDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SkillGapAnalysisDto>> AnalyzeSkillGaps(Guid studyPlanId)
        {
            try
            {
                var userId = GetUserId();
                _logger.LogInformation($"?? User {userId} analyzing skill gaps for plan {studyPlanId}");
                
                var analysis = await _studyPlanService.AnalyzeSkillGapsAsync(userId, studyPlanId);
                return Ok(analysis);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access to study plan");
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid skill gap analysis operation");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing skill gaps");
                return StatusCode(500, new { message = "Error analyzing skill gaps", error = ex.Message });
            }
        }

        /// <summary>
        /// Activate study plan per SRS 3.2
        /// </summary>
        /// <param name="studyPlanId">Study plan ID</param>
        /// <returns>Success result</returns>
        /// <response code="200">Study plan activated successfully</response>
        /// <response code="403">Access denied</response>
        /// <response code="404">Study plan not found</response>
        [HttpPost("{studyPlanId}/activate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ActivateStudyPlan(Guid studyPlanId)
        {
            try
            {
                var userId = GetUserId();
                _logger.LogInformation($"? User {userId} activating study plan {studyPlanId}");
                
                await _studyPlanService.ActivateStudyPlanAsync(userId, studyPlanId);
                return Ok(new { message = "Study plan activated successfully" });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access to study plan");
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating study plan");
                return StatusCode(500, new { message = "Error activating study plan", error = ex.Message });
            }
        }

        /// <summary>
        /// Archive study plan
        /// </summary>
        /// <param name="studyPlanId">Study plan ID</param>
        /// <returns>Success result</returns>
        /// <response code="200">Study plan archived successfully</response>
        /// <response code="403">Access denied</response>
        /// <response code="404">Study plan not found</response>
        [HttpPost("{studyPlanId}/archive")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ArchiveStudyPlan(Guid studyPlanId)
        {
            try
            {
                var userId = GetUserId();
                _logger.LogInformation($"?? User {userId} archiving study plan {studyPlanId}");
                
                await _studyPlanService.ArchiveStudyPlanAsync(userId, studyPlanId);
                return Ok(new { message = "Study plan archived successfully" });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access to study plan");
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error archiving study plan");
                return StatusCode(500, new { message = "Error archiving study plan", error = ex.Message });
            }
        }
    }
}
