using Mentora.Application.DTOs.StudyPlanner;
using Mentora.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mentora.Api.Controllers
{
    /// <summary>
    /// PlannerController for Study Planner Module
    /// Per SRS Study Planner - Feature 3: Planner (Calendar & Events)
    /// FR-PL-01 to FR-PL-06
    /// </summary>
    [Authorize]
    [Route("api/planner")]
    [ApiController]
    public class PlannerController : BaseController
    {
        private readonly IPlannerService _plannerService;

        public PlannerController(IPlannerService plannerService)
        {
            _plannerService = plannerService;
        }

        /// <summary>
        /// Create a new event
        /// POST /api/planner/events
        /// </summary>
        [HttpPost("events")]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto dto)
        {
            try
            {
                var userId = GetUserId();
                var plannerEvent = await _plannerService.CreateEventAsync(userId, dto);
                return Ok(new
                {
                    success = true,
                    message = "Event created successfully",
                    data = plannerEvent
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Get events for a specific date
        /// GET /api/planner/events?date=YYYY-MM-DD
        /// </summary>
        [HttpGet("events")]
        public async Task<IActionResult> GetEventsByDate([FromQuery] string? date)
        {
            try
            {
                var userId = GetUserId();
                
                IEnumerable<PlannerEventDto> events;
                
                if (string.IsNullOrEmpty(date))
                {
                    events = await _plannerService.GetUserEventsAsync(userId);
                }
                else
                {
                    events = await _plannerService.GetEventsByDateAsync(userId, date);
                }

                return Ok(new { success = true, data = events });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Get upcoming events
        /// GET /api/planner/events/upcoming
        /// </summary>
        [HttpGet("events/upcoming")]
        public async Task<IActionResult> GetUpcomingEvents()
        {
            var userId = GetUserId();
            var events = await _plannerService.GetUpcomingEventsAsync(userId);
            return Ok(new { success = true, data = events });
        }

        /// <summary>
        /// Mark event as attended
        /// PATCH /api/planner/events/{id}/attend
        /// </summary>
        [HttpPatch("events/{id}/attend")]
        public async Task<IActionResult> MarkEventAttended(Guid id)
        {
            var userId = GetUserId();
            var plannerEvent = await _plannerService.MarkAttendedAsync(id, userId);

            if (plannerEvent == null)
            {
                return NotFound(new { success = false, message = "Event not found" });
            }

            return Ok(new
            {
                success = true,
                message = "Event marked as attended",
                data = plannerEvent
            });
        }

        /// <summary>
        /// Delete an event
        /// DELETE /api/planner/events/{id}
        /// </summary>
        [HttpDelete("events/{id}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var userId = GetUserId();
            var success = await _plannerService.DeleteEventAsync(id, userId);

            if (!success)
            {
                return NotFound(new { success = false, message = "Event not found" });
            }

            return Ok(new { success = true, message = "Event deleted successfully" });
        }
    }
}
