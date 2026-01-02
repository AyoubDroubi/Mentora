using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mentora.Application.DTOs.CareerBuilder;
using Mentora.Application.Interfaces.Services;
using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities;
using System.Security.Claims;
using System.Text.Json;

namespace Mentora.Api.Controllers
{
    [ApiController]
    [Route("api/career-quiz")]
    [Authorize]
    public class CareerQuizController : ControllerBase
    {
        private readonly ICareerPlanService _careerPlanService;
        private readonly ICareerQuizRepository _quizRepository;
        private readonly ICareerPlanRepository _planRepository;
        private readonly ILogger<CareerQuizController> _logger;

        public CareerQuizController(
            ICareerPlanService careerPlanService,
            ICareerQuizRepository quizRepository,
            ICareerPlanRepository planRepository,
            ILogger<CareerQuizController> logger)
        {
            _careerPlanService = careerPlanService;
            _quizRepository = quizRepository;
            _planRepository = planRepository;
            _logger = logger;
        }

        /// <summary>
        /// Get standardized quiz questions
        /// GET /api/career-quiz/questions
        /// Per SRS FR-QZ-01: Standardized questions for all users
        /// </summary>
        [HttpGet("questions")]
        public IActionResult GetQuestions()
        {
            try
            {
                var questions = GetStandardizedQuestions();
                return Ok(questions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Error retrieving quiz questions");
                return StatusCode(500, new { message = "An error occurred" });
            }
        }

        /// <summary>
        /// Save quiz as draft
        /// POST /api/career-quiz/save-draft
        /// Per SRS FR-QZ-02: Allow saving as Draft
        /// </summary>
        [HttpPost("save-draft")]
        public async Task<IActionResult> SaveDraft([FromBody] QuizSubmissionDto dto)
        {
            try
            {
                var userId = GetUserId();
                if (userId == Guid.Empty)
                    return Unauthorized(new { message = "Invalid user token" });

                _logger.LogInformation($"?? Saving quiz draft for user: {userId}");

                var answersJson = JsonSerializer.Serialize(dto.Answers);

                var quiz = new CareerQuizAttempt
                {
                    UserId = userId,
                    AnswersJson = answersJson,
                    Status = CareerQuizStatus.Draft,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _quizRepository.CreateAsync(quiz);

                return Ok(new
                {
                    success = true,
                    message = "Quiz saved as draft",
                    quizId = quiz.Id
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Error saving quiz draft");
                return StatusCode(500, new { success = false, message = "An error occurred" });
            }
        }

        /// <summary>
        /// Submit quiz and generate career plan
        /// POST /api/career-quiz/submit
        /// Per SRS FR-QZ-03: Submitting sets Completed
        /// Per SRS FR-QZ-04: Enable "Generate Plan"
        /// Per SRS FR-QZ-05: Outdate previous quiz and plans
        /// </summary>
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitQuiz([FromBody] QuizSubmissionDto dto)
        {
            try
            {
                var userId = GetUserId();
                if (userId == Guid.Empty)
                    return Unauthorized(new { message = "Invalid user token" });

                _logger.LogInformation($"?? Quiz submission received from user: {userId}");

                // FR-QZ-05: Outdate previous quizzes and linked plans
                await OutdatePreviousQuizzesAndPlans(userId);

                // Save quiz as Completed
                var answersJson = JsonSerializer.Serialize(dto.Answers);
                var quiz = new CareerQuizAttempt
                {
                    UserId = userId,
                    AnswersJson = answersJson,
                    Status = CareerQuizStatus.Completed,
                    SubmittedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                quiz = await _quizRepository.CreateAsync(quiz);
                _logger.LogInformation($"? Quiz saved with ID: {quiz.Id}");

                // Generate career plan using AI service
                var generateDto = new GenerateCareerPlanDto
                {
                    QuizAttemptId = quiz.Id
                };

                var result = await _careerPlanService.GenerateCareerPlanAsync(userId, generateDto);

                if (result.Success)
                {
                    _logger.LogInformation($"? Career plan generated successfully for user: {userId}");
                    return Ok(new
                    {
                        success = true,
                        message = "Career plan generated successfully!",
                        quizId = quiz.Id,
                        planId = result.CareerPlan?.Id,
                        plan = result.CareerPlan
                    });
                }
                else
                {
                    _logger.LogWarning($"?? Failed to generate career plan: {result.Message}");
                    return BadRequest(new
                    {
                        success = false,
                        message = result.Message,
                        quizId = quiz.Id
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Error processing quiz submission");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while processing your quiz. Please try again."
                });
            }
        }

        /// <summary>
        /// Get user's latest quiz attempt
        /// GET /api/career-quiz/latest
        /// </summary>
        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestQuiz()
        {
            try
            {
                var userId = GetUserId();
                if (userId == Guid.Empty)
                    return Unauthorized();

                var quizzes = await _quizRepository.GetByUserIdAsync(userId);
                var latestQuiz = quizzes.OrderByDescending(q => q.CreatedAt).FirstOrDefault();

                if (latestQuiz == null)
                    return NotFound(new { message = "No quiz attempts found" });

                return Ok(new
                {
                    quizId = latestQuiz.Id,
                    status = latestQuiz.Status.ToString(),
                    createdAt = latestQuiz.CreatedAt,
                    submittedAt = latestQuiz.SubmittedAt,
                    answers = JsonSerializer.Deserialize<Dictionary<string, object>>(latestQuiz.AnswersJson)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Error retrieving latest quiz");
                return StatusCode(500, new { message = "An error occurred" });
            }
        }

        /// <summary>
        /// Outdate previous quizzes and their linked career plans
        /// Per SRS FR-QZ-05
        /// </summary>
        private async Task OutdatePreviousQuizzesAndPlans(Guid userId)
        {
            try
            {
                // Get all previous quizzes
                var previousQuizzes = await _quizRepository.GetByUserIdAsync(userId);
                
                foreach (var quiz in previousQuizzes.Where(q => q.Status == CareerQuizStatus.Completed))
                {
                    // Mark quiz as Outdated
                    quiz.Status = CareerQuizStatus.Outdated;
                    quiz.UpdatedAt = DateTime.UtcNow;
                    await _quizRepository.UpdateAsync(quiz);
                }

                // Mark linked career plans as Outdated
                var allPlans = await _planRepository.GetAllByUserIdAsync(userId);
                foreach (var plan in allPlans.Where(p => p.Status != CareerPlanStatus.Outdated))
                {
                    plan.Status = CareerPlanStatus.Outdated;
                    plan.UpdatedAt = DateTime.UtcNow;
                    await _planRepository.UpdateAsync(plan);
                }

                _logger.LogInformation($"?? Outdated {previousQuizzes.Count()} quizzes and {allPlans.Count()} plans");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Error outdating previous quizzes/plans");
            }
        }

        /// <summary>
        /// Get standardized quiz questions
        /// Per SRS FR-QZ-01
        /// </summary>
        private object GetStandardizedQuestions()
        {
            var questions = new object[]
            {
                new { id = "q1", type = "short_answer", question = "What career field are you most interested in?", placeholder = "e.g., Software Developer, Data Scientist..." },
                new { id = "q2", type = "short_answer", question = "Why does this career appeal to you?", placeholder = "e.g., I enjoy solving problems..." },
                new { id = "q3", type = "multiple_choice", maxSelections = 2, question = "Which industries attract you?", options = new[] { "Technology", "Finance", "Healthcare", "Education", "Other" } },
                new { id = "q4", type = "multiple_choice", maxSelections = 3, question = "What are your top strengths?", options = new[] { "Problem-solving", "Creativity", "Communication", "Leadership", "Technical skills" } },
                new { id = "q5", type = "short_answer", question = "What skills do you already have?", placeholder = "e.g., JavaScript, Python..." },
                new { id = "q6", type = "multiple_choice", question = "Which skills do you need to improve?", options = new[] { "Programming", "System design", "Communication", "Time management", "Teamwork" } }
            };
            
            return new { questions };
        }

        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }
    }
}
