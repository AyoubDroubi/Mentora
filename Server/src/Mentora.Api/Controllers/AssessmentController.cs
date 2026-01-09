using Mentora.Application.DTOs.Assessment;
using Mentora.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mentora.Api.Controllers
{
    /// <summary>
    /// Assessment API endpoints per SRS 3.1
    /// Provides dynamic diagnostic questions and response management
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AssessmentController : BaseController
    {
        private readonly IAssessmentService _assessmentService;
        private readonly ILogger<AssessmentController> _logger;

        public AssessmentController(
            IAssessmentService assessmentService,
            ILogger<AssessmentController> logger)
        {
            _assessmentService = assessmentService;
            _logger = logger;
        }

        /// <summary>
        /// Get assessment questions by major per SRS 3.1.1
        /// </summary>
        /// <param name="targetMajor">Optional: Student's major to filter questions</param>
        /// <returns>List of assessment questions</returns>
        /// <response code="200">Questions retrieved successfully</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("questions")]
        [ProducesResponseType(typeof(List<AssessmentQuestionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<AssessmentQuestionDto>>> GetQuestions([FromQuery] string? targetMajor = null)
        {
            try
            {
                _logger.LogInformation($"?? Getting assessment questions for major: {targetMajor ?? "All"}");
                var questions = await _assessmentService.GetQuestionsByMajorAsync(targetMajor);
                return Ok(questions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving assessment questions");
                return StatusCode(500, new { message = "Error retrieving questions", error = ex.Message });
            }
        }

        /// <summary>
        /// Start new assessment attempt per SRS 3.1
        /// </summary>
        /// <param name="major">Student's major</param>
        /// <param name="studyLevel">Student's study level (Freshman, Sophomore, etc.)</param>
        /// <returns>Assessment attempt details</returns>
        /// <response code="201">Assessment started successfully</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("start")]
        [ProducesResponseType(typeof(AssessmentAttemptDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AssessmentAttemptDto>> StartAssessment(
            [FromQuery] string? major,
            [FromQuery] string? studyLevel)
        {
            try
            {
                // Validate required parameters
                if (string.IsNullOrWhiteSpace(major))
                {
                    return BadRequest(new { message = "Major is required" });
                }
                
                if (string.IsNullOrWhiteSpace(studyLevel))
                {
                    return BadRequest(new { message = "Study level is required" });
                }
                
                var userId = GetUserId();
                _logger.LogInformation($"?? User {userId} starting assessment for {major}");
                
                var attempt = await _assessmentService.StartAssessmentAsync(userId, major, studyLevel);
                return CreatedAtAction(nameof(GetAttempt), new { attemptId = attempt.Id }, attempt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting assessment");
                return StatusCode(500, new { message = "Error starting assessment", error = ex.Message });
            }
        }

        /// <summary>
        /// Get assessment attempt details
        /// </summary>
        /// <param name="attemptId">Assessment attempt ID</param>
        /// <returns>Assessment attempt details</returns>
        /// <response code="200">Attempt retrieved successfully</response>
        /// <response code="404">Attempt not found</response>
        [HttpGet("attempts/{attemptId}")]
        [ProducesResponseType(typeof(AssessmentAttemptDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AssessmentAttemptDto>> GetAttempt(Guid attemptId)
        {
            try
            {
                var userId = GetUserId();
                // Implementation would fetch from repository
                return Ok(new { message = "Not implemented yet" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving assessment attempt");
                return StatusCode(500, new { message = "Error retrieving attempt", error = ex.Message });
            }
        }

        /// <summary>
        /// Submit single assessment response per SRS 3.1.2
        /// </summary>
        /// <param name="attemptId">Assessment attempt ID</param>
        /// <param name="response">Response data</param>
        /// <returns>Assessment completion status</returns>
        /// <response code="200">Response submitted successfully</response>
        /// <response code="400">Invalid response data</response>
        /// <response code="403">Access denied</response>
        [HttpPost("attempts/{attemptId}/responses")]
        [ProducesResponseType(typeof(AssessmentCompletionResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<AssessmentCompletionResponseDto>> SubmitResponse(
            Guid attemptId,
            [FromBody] SubmitAssessmentResponseDto response)
        {
            try
            {
                var userId = GetUserId();
                _logger.LogInformation($"?? User {userId} submitting response for question {response.QuestionId}");
                
                var result = await _assessmentService.SubmitResponseAsync(userId, attemptId, response);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access to assessment");
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting response");
                return StatusCode(500, new { message = "Error submitting response", error = ex.Message });
            }
        }

        /// <summary>
        /// Bulk submit assessment responses per SRS 3.1.2
        /// </summary>
        /// <param name="submission">Bulk submission data</param>
        /// <returns>Assessment completion status</returns>
        /// <response code="200">Responses submitted successfully</response>
        /// <response code="400">Invalid submission data</response>
        /// <response code="403">Access denied</response>
        /// <response code="404">Assessment attempt not found</response>
        [HttpPost("responses/bulk")]
        [ProducesResponseType(typeof(AssessmentCompletionResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AssessmentCompletionResponseDto>> BulkSubmitResponses(
            [FromBody] BulkAssessmentSubmissionDto submission)
        {
            try
            {
                var userId = GetUserId();
                _logger.LogInformation($"?? User {userId} bulk submitting {submission.Responses.Count} responses for attempt {submission.AssessmentAttemptId}");
                
                var result = await _assessmentService.BulkSubmitResponsesAsync(userId, submission);
                
                _logger.LogInformation($"? Bulk submission successful - {result.AnsweredQuestions}/{result.TotalQuestions} answered");
                
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation during bulk submission");
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access to assessment");
                return StatusCode(403, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error bulk submitting responses");
                return StatusCode(500, new { message = "Error submitting responses", error = ex.Message });
            }
        }

        /// <summary>
        /// Complete assessment and generate context per SRS 3.1.2
        /// </summary>
        /// <param name="attemptId">Assessment attempt ID</param>
        /// <returns>Assessment completion result with serialized context</returns>
        /// <response code="200">Assessment completed successfully</response>
        /// <response code="403">Access denied</response>
        [HttpPost("attempts/{attemptId}/complete")]
        [ProducesResponseType(typeof(AssessmentCompletionResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<AssessmentCompletionResponseDto>> CompleteAssessment(Guid attemptId)
        {
            try
            {
                var userId = GetUserId();
                _logger.LogInformation($"? User {userId} completing assessment {attemptId}");
                
                var result = await _assessmentService.CompleteAssessmentAsync(userId, attemptId);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access to assessment");
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid assessment operation");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing assessment");
                return StatusCode(500, new { message = "Error completing assessment", error = ex.Message });
            }
        }

        /// <summary>
        /// Get assessment context for AI prompt injection per SRS 3.1.2
        /// </summary>
        /// <param name="attemptId">Assessment attempt ID</param>
        /// <returns>Assessment context DTO</returns>
        /// <response code="200">Context retrieved successfully</response>
        /// <response code="404">Context not found</response>
        [HttpGet("attempts/{attemptId}/context")]
        [ProducesResponseType(typeof(AssessmentContextDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AssessmentContextDto>> GetContext(Guid attemptId)
        {
            try
            {
                _logger.LogInformation($"?? Retrieving context for attempt {attemptId}");
                
                var context = await _assessmentService.GetAssessmentContextAsync(attemptId);
                if (context == null)
                    return NotFound(new { message = "Assessment context not found" });

                return Ok(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving assessment context");
                return StatusCode(500, new { message = "Error retrieving context", error = ex.Message });
            }
        }
    }
}
