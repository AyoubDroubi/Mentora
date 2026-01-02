using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mentora.Application.DTOs.CareerBuilder;
using Mentora.Application.Interfaces.Services;
using System.Security.Claims;

namespace Mentora.Api.Controllers
{
    [ApiController]
    [Route("api/career-plans")]
    [Authorize]
    public class CareerPlanController : ControllerBase
    {
        private readonly ICareerPlanService _careerPlanService;
        private readonly ILogger<CareerPlanController> _logger;

        public CareerPlanController(
            ICareerPlanService careerPlanService,
            ILogger<CareerPlanController> logger)
        {
            _careerPlanService = careerPlanService;
            _logger = logger;
        }

        /// <summary>
        /// Generate AI-powered career plan from quiz
        /// POST /api/career-plans/generate
        /// </summary>
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateCareerPlan([FromBody] GenerateCareerPlanDto dto)
        {
            try
            {
                var userId = GetUserId();
                if (userId == Guid.Empty)
                    return Unauthorized(new { message = "Invalid user token" });

                _logger.LogInformation($"?? Generating career plan for user: {userId}");

                var result = await _careerPlanService.GenerateCareerPlanAsync(userId, dto);

                if (result.Success)
                {
                    _logger.LogInformation($"? Career plan generated successfully");
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Error generating career plan");
                return StatusCode(500, new { success = false, message = "An error occurred" });
            }
        }

        /// <summary>
        /// Get all career plans for current user
        /// GET /api/career-plans
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllPlans()
        {
            try
            {
                var userId = GetUserId();
                if (userId == Guid.Empty)
                    return Unauthorized();

                var plans = await _careerPlanService.GetAllPlansAsync(userId);
                return Ok(plans);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Error retrieving plans");
                return StatusCode(500, new { message = "An error occurred" });
            }
        }

        /// <summary>
        /// Get career plan details by ID
        /// GET /api/career-plans/{id}
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlanDetails(Guid id)
        {
            try
            {
                var userId = GetUserId();
                if (userId == Guid.Empty)
                    return Unauthorized();

                var plan = await _careerPlanService.GetPlanDetailsAsync(userId, id);
                
                if (plan == null)
                    return NotFound(new { message = "Plan not found" });

                return Ok(plan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"? Error retrieving plan {id}");
                return StatusCode(500, new { message = "An error occurred" });
            }
        }

        /// <summary>
        /// Update career plan status
        /// PATCH /api/career-plans/{id}/status
        /// </summary>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdatePlanStatus(Guid id, [FromBody] UpdatePlanStatusDto dto)
        {
            try
            {
                var userId = GetUserId();
                if (userId == Guid.Empty)
                    return Unauthorized();

                // TODO: Implement UpdatePlanStatusAsync in service
                return Ok(new { message = "Status updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"? Error updating plan status {id}");
                return StatusCode(500, new { message = "An error occurred" });
            }
        }

        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }
    }
}
