using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities;
using System.Security.Claims;

namespace Mentora.Api.Controllers
{
    [ApiController]
    [Route("api/career-progress")]
    [Authorize]
    public class CareerProgressController : ControllerBase
    {
        private readonly ICareerPlanRepository _planRepository;
        private readonly ICareerPlanSkillRepository _skillRepository;
        private readonly ILogger<CareerProgressController> _logger;

        public CareerProgressController(
            ICareerPlanRepository planRepository,
            ICareerPlanSkillRepository skillRepository,
            ILogger<CareerProgressController> logger)
        {
            _planRepository = planRepository;
            _skillRepository = skillRepository;
            _logger = logger;
        }

        /// <summary>
        /// Get active career plan progress
        /// GET /api/career-progress/active
        /// Per SRS FR-PR-01: Display active plan progress
        /// Per SRS FR-PR-02: Show step name and progress
        /// Per SRS FR-PR-03: Auto-calculated progress
        /// Per SRS FR-PR-04: Read-only view
        /// </summary>
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveProgress()
        {
            try
            {
                var userId = GetUserId();
                if (userId == Guid.Empty)
                    return Unauthorized();

                // Get active career plan (InProgress or most recent Generated)
                var allPlans = await _planRepository.GetAllByUserIdAsync(userId);
                var activePlan = allPlans
                    .Where(p => !p.IsDeleted && p.Status != CareerPlanStatus.Outdated)
                    .OrderByDescending(p => p.Status == CareerPlanStatus.InProgress ? 1 : 0)
                    .ThenByDescending(p => p.CreatedAt)
                    .FirstOrDefault();

                if (activePlan == null)
                {
                    return Ok(new
                    {
                        hasActivePlan = false,
                        message = "No active career plan found"
                    });
                }

                // Get plan with all details
                var planDetails = await _planRepository.GetByIdWithStepsAsync(activePlan.Id);
                if (planDetails == null)
                    return NotFound();

                // Get all skills for progress calculation
                var allSkills = await _skillRepository.GetByPlanIdAsync(activePlan.Id);

                // Build progress response
                var stepsProgress = planDetails.Steps
                    .OrderBy(s => s.OrderIndex)
                    .Select(step =>
                    {
                        var stepSkills = allSkills.Where(s => s.CareerStepId == step.Id).ToList();
                        
                        return new
                        {
                            id = step.Id,
                            name = step.Title,
                            description = step.Description,
                            orderIndex = step.OrderIndex,
                            status = step.Status.ToString(),
                            progressPercentage = step.ProgressPercentage,
                            skillsCount = stepSkills.Count,
                            achievedSkills = stepSkills.Count(s => s.Status == SkillStatus.Achieved),
                            inProgressSkills = stepSkills.Count(s => s.Status == SkillStatus.InProgress),
                            missingSkills = stepSkills.Count(s => s.Status == SkillStatus.Missing)
                        };
                    })
                    .ToList();

                var result = new
                {
                    hasActivePlan = true,
                    planId = planDetails.Id,
                    planTitle = planDetails.Title,
                    planSummary = planDetails.Summary,
                    planStatus = planDetails.Status.ToString(),
                    overallProgress = planDetails.ProgressPercentage,
                    createdAt = planDetails.CreatedAt,
                    steps = stepsProgress,
                    totalSteps = stepsProgress.Count,
                    completedSteps = stepsProgress.Count(s => s.progressPercentage == 100),
                    inProgressSteps = stepsProgress.Count(s => s.progressPercentage > 0 && s.progressPercentage < 100),
                    pendingSteps = stepsProgress.Count(s => s.progressPercentage == 0)
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Error retrieving career progress");
                return StatusCode(500, new { message = "An error occurred" });
            }
        }

        /// <summary>
        /// Get progress history for all plans
        /// GET /api/career-progress/history
        /// </summary>
        [HttpGet("history")]
        public async Task<IActionResult> GetProgressHistory()
        {
            try
            {
                var userId = GetUserId();
                if (userId == Guid.Empty)
                    return Unauthorized();

                var allPlans = await _planRepository.GetAllByUserIdAsync(userId);

                var history = allPlans
                    .OrderByDescending(p => p.CreatedAt)
                    .Select(p => new
                    {
                        planId = p.Id,
                        title = p.Title,
                        status = p.Status.ToString(),
                        progress = p.ProgressPercentage,
                        createdAt = p.CreatedAt,
                        isActive = p.IsActive && !p.IsDeleted && p.Status != CareerPlanStatus.Outdated
                    })
                    .ToList();

                return Ok(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Error retrieving progress history");
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
