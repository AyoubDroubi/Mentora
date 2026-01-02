using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mentora.Application.Interfaces.Repositories;
using Mentora.Application.DTOs.CareerBuilder;
using Mentora.Domain.Entities;
using System.Security.Claims;

namespace Mentora.Api.Controllers
{
    [ApiController]
    [Route("api/career-plans/{planId}/skills")]
    [Authorize]
    public class SkillsController : ControllerBase
    {
        private readonly ICareerPlanSkillRepository _skillRepository;
        private readonly ICareerPlanRepository _planRepository;
        private readonly ILogger<SkillsController> _logger;

        public SkillsController(
            ICareerPlanSkillRepository skillRepository,
            ICareerPlanRepository planRepository,
            ILogger<SkillsController> logger)
        {
            _skillRepository = skillRepository;
            _planRepository = planRepository;
            _logger = logger;
        }

        /// <summary>
        /// Get all skills for a career plan
        /// GET /api/career-plans/{planId}/skills
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetPlanSkills(Guid planId)
        {
            try
            {
                var userId = GetUserId();
                if (userId == Guid.Empty)
                    return Unauthorized();

                // Verify plan belongs to user
                var plan = await _planRepository.GetByIdAndUserIdAsync(planId, userId);
                if (plan == null)
                    return NotFound(new { message = "Plan not found" });

                var skills = await _skillRepository.GetByPlanIdAsync(planId);

                var result = skills.Select(s => new
                {
                    id = s.Id,
                    skillId = s.SkillId,
                    skillName = s.Skill.Name,
                    category = s.Skill.Category.ToString(),
                    status = s.Status.ToString(),
                    targetLevel = s.TargetLevel.ToString(),
                    stepId = s.CareerStepId,
                    stepName = s.CareerStep?.Title
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"? Error retrieving skills for plan {planId}");
                return StatusCode(500, new { message = "An error occurred" });
            }
        }

        /// <summary>
        /// Update skill status
        /// PATCH /api/career-plans/{planId}/skills/{skillId}
        /// Per SRS FR-SK-03: User can update skill status
        /// Per SRS FR-SK-04: Auto-updates step and plan progress
        /// </summary>
        [HttpPatch("{skillId}")]
        public async Task<IActionResult> UpdateSkillStatus(Guid planId, Guid skillId, [FromBody] UpdateSkillStatusDto dto)
        {
            try
            {
                var userId = GetUserId();
                if (userId == Guid.Empty)
                    return Unauthorized();

                // Verify plan belongs to user
                var plan = await _planRepository.GetByIdAndUserIdAsync(planId, userId);
                if (plan == null)
                    return NotFound(new { message = "Plan not found" });

                // Get the skill
                var skill = await _skillRepository.GetByIdAsync(skillId);
                if (skill == null || skill.CareerPlanId != planId)
                    return NotFound(new { message = "Skill not found" });

                // Parse and validate status
                if (!Enum.TryParse<SkillStatus>(dto.Status, true, out var newStatus))
                    return BadRequest(new { message = "Invalid status value" });

                // Update skill status
                skill.Status = newStatus;
                await _skillRepository.UpdateAsync(skill);

                _logger.LogInformation($"? Skill {skillId} status updated to {newStatus}");

                // FR-SK-04: Auto-update progress
                await UpdateProgressCascade(planId);

                return Ok(new
                {
                    success = true,
                    message = "Skill status updated successfully",
                    skillId = skillId,
                    newStatus = newStatus.ToString()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"? Error updating skill {skillId}");
                return StatusCode(500, new { message = "An error occurred" });
            }
        }

        /// <summary>
        /// Auto-update step and plan progress when skill changes
        /// Per SRS: Skills ? Steps ? Plan progress propagation
        /// </summary>
        private async Task UpdateProgressCascade(Guid planId)
        {
            try
            {
                // Get plan with all steps and skills
                var plan = await _planRepository.GetByIdWithStepsAsync(planId);
                if (plan == null) return;

                // Get all skills for the plan
                var allSkills = await _skillRepository.GetByPlanIdAsync(planId);

                // Update each step's progress based on its skills
                foreach (var step in plan.Steps)
                {
                    var stepSkills = allSkills.Where(s => s.CareerStepId == step.Id).ToList();
                    if (stepSkills.Any())
                    {
                        var achievedCount = stepSkills.Count(s => s.Status == SkillStatus.Achieved);
                        var inProgressCount = stepSkills.Count(s => s.Status == SkillStatus.InProgress);
                        
                        step.ProgressPercentage = (achievedCount * 100 + inProgressCount * 50) / stepSkills.Count;
                    }
                }

                // Update plan's overall progress
                if (plan.Steps.Any())
                {
                    plan.ProgressPercentage = (int)plan.Steps.Average(s => s.ProgressPercentage);
                }

                // Update plan status based on progress
                if (plan.ProgressPercentage == 100)
                {
                    plan.Status = CareerPlanStatus.Completed;
                }
                else if (plan.ProgressPercentage > 0 && plan.Status == CareerPlanStatus.Generated)
                {
                    plan.Status = CareerPlanStatus.InProgress;
                }

                await _planRepository.UpdateAsync(plan);

                _logger.LogInformation($"?? Progress updated - Plan: {plan.ProgressPercentage}%");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Error updating progress cascade");
            }
        }

        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }
    }
}
