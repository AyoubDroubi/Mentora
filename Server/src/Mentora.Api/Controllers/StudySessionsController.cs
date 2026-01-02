using Mentora.Application.DTOs.StudyPlanner;
using Mentora.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mentora.Api.Controllers
{
    /// <summary>
    /// StudySessionsController for Study Planner Module
    /// Per SRS Study Planner - Feature 1: Pomodoro Timer (Study Sessions)
    /// FR-PT-01 to FR-PT-06
    /// </summary>
    [Authorize]
    [Route("api/study-sessions")]
    [ApiController]
    public class StudySessionsController : BaseController
    {
        private readonly IStudySessionsService _sessionsService;

        public StudySessionsController(IStudySessionsService sessionsService)
        {
            _sessionsService = sessionsService;
        }

        /// <summary>
        /// Save a completed Pomodoro session
        /// POST /api/study-sessions
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SaveSession([FromBody] SaveSessionDto dto)
        {
            try
            {
                var userId = GetUserId();
                var session = await _sessionsService.SaveSessionAsync(userId, dto);
                return Ok(new
                {
                    success = true,
                    message = "Study session saved successfully",
                    data = session
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Get study time summary (total minutes and formatted time)
        /// GET /api/study-sessions/summary
        /// </summary>
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var userId = GetUserId();
            var summary = await _sessionsService.GetSummaryAsync(userId);
            return Ok(new { success = true, data = summary });
        }

        /// <summary>
        /// Get all study sessions for the authenticated user
        /// GET /api/study-sessions
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSessions([FromQuery] int limit = 50)
        {
            var userId = GetUserId();
            var sessions = await _sessionsService.GetSessionsAsync(userId, limit);
            return Ok(new { success = true, data = sessions });
        }

        /// <summary>
        /// Get study sessions by date range
        /// GET /api/study-sessions/range?from=YYYY-MM-DD&to=YYYY-MM-DD
        /// </summary>
        [HttpGet("range")]
        public async Task<IActionResult> GetSessionsByRange([FromQuery] string from, [FromQuery] string to)
        {
            try
            {
                var userId = GetUserId();
                var result = await _sessionsService.GetSessionsByRangeAsync(userId, from, to);
                return Ok(new { success = true, data = result });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Delete a study session
        /// DELETE /api/study-sessions/{id}
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSession(Guid id)
        {
            var userId = GetUserId();
            var success = await _sessionsService.DeleteSessionAsync(id, userId);

            if (!success)
            {
                return NotFound(new { success = false, message = "Study session not found" });
            }

            return Ok(new { success = true, message = "Study session deleted successfully" });
        }
    }
}
