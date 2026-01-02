using Mentora.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mentora.Api.Controllers
{
    /// <summary>
    /// StudyQuizController for Study Planner Module
    /// Per SRS Study Planner - Feature 6: Study Quiz (Diagnostic)
    /// FR-QZ-01 to FR-QZ-03
    /// </summary>
    [Authorize]
    [Route("api/study-quiz")]
    [ApiController]
    public class StudyQuizController : BaseController
    {
        private readonly IStudyQuizService _quizService;

        public StudyQuizController(IStudyQuizService quizService)
        {
            _quizService = quizService;
        }

        /// <summary>
        /// Get standardized quiz questions
        /// GET /api/study-quiz/questions
        /// </summary>
        [HttpGet("questions")]
        public async Task<IActionResult> GetQuestions()
        {
            var questions = await _quizService.GetQuestionsAsync();
            return Ok(new { success = true, data = questions });
        }

        /// <summary>
        /// Submit quiz answers and generate personalized study plan
        /// POST /api/study-quiz/submit
        /// </summary>
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitQuiz([FromBody] Dictionary<string, string> answers)
        {
            try
            {
                var userId = GetUserId();
                var result = await _quizService.SubmitQuizAsync(userId, answers);
                return Ok(new
                {
                    success = true,
                    message = "Quiz submitted successfully",
                    data = result
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Get the latest quiz attempt
        /// GET /api/study-quiz/latest
        /// </summary>
        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestAttempt()
        {
            var userId = GetUserId();
            var attempt = await _quizService.GetLatestAttemptAsync(userId);

            if (attempt == null)
            {
                return NotFound(new { success = false, message = "No quiz attempts found" });
            }

            return Ok(new { success = true, data = attempt });
        }
    }
}
