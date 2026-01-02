using Mentora.Application.Interfaces.Services;
using Mentora.Api.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mentora.Api.Controllers
{
    /// <summary>
    /// AttendanceController for Study Planner Module
    /// Per SRS Study Planner - Feature 4: Attendance & Overall Progress Percentage
    /// FR-AT-01 to FR-AT-02
    /// </summary>
    [Authorize]
    [Route("api/study-planner/attendance")]
    [ApiController]
    public class AttendanceController : BaseController
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        /// <summary>
        /// Get attendance and overall progress summary
        /// GET /api/study-planner/attendance/summary
        /// Progress Calculation:
        /// - 50% from completed tasks
        /// - 50% from attended events
        /// </summary>
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var userId = GetUserId();
            var summary = await _attendanceService.GetSummaryAsync(userId);
            return Ok(new { success = true, data = summary });
        }

        /// <summary>
        /// Get detailed attendance history
        /// GET /api/study-planner/attendance/history?days=30
        /// </summary>
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory([FromQuery] int days = 30)
        {
            var userId = GetUserId();
            var history = await _attendanceService.GetHistoryAsync(userId, days);
            return Ok(new { success = true, data = history });
        }

        /// <summary>
        /// Get weekly progress report
        /// GET /api/study-planner/attendance/weekly
        /// </summary>
        [HttpGet("weekly")]
        public async Task<IActionResult> GetWeeklyProgress()
        {
            var userId = GetUserId();
            var weeklyProgress = await _attendanceService.GetWeeklyProgressAsync(userId);
            return Ok(new { success = true, data = weeklyProgress });
        }
    }
}
