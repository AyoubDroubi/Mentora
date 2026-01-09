using Mentora.Application.DTOs.Assessment;
using Mentora.Application.Interfaces.Repositories;
using Mentora.Application.Services.AI;
using Mentora.Domain.Entities;
using Mentora.Domain.Entities.Assessment;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Mentora.Application.Services
{
    /// <summary>
    /// Service for managing assessment questions and attempts per SRS 3.1
    /// </summary>
    public interface IAssessmentService
    {
        Task<List<AssessmentQuestionDto>> GetQuestionsByMajorAsync(string major);
        Task<AssessmentAttemptDto> StartAssessmentAsync(Guid userId, string major, string studyLevel);
        Task<AssessmentCompletionResponseDto> SubmitResponseAsync(Guid userId, Guid attemptId, SubmitAssessmentResponseDto response);
        Task<AssessmentCompletionResponseDto> BulkSubmitResponsesAsync(Guid userId, BulkAssessmentSubmissionDto submission);
        Task<AssessmentCompletionResponseDto> CompleteAssessmentAsync(Guid userId, Guid attemptId);
        Task<AssessmentContextDto?> GetAssessmentContextAsync(Guid attemptId);
    }

    /// <summary>
    /// Implementation of assessment service per SRS 3.1
    /// </summary>
    public class AssessmentService : IAssessmentService
    {
        private readonly IAssessmentRepository _assessmentRepo;
        private readonly AssessmentContextBuilder _contextBuilder;
        private readonly ILogger<AssessmentService> _logger;

        public AssessmentService(
            IAssessmentRepository assessmentRepo,
            AssessmentContextBuilder contextBuilder,
            ILogger<AssessmentService> logger)
        {
            _assessmentRepo = assessmentRepo;
            _contextBuilder = contextBuilder;
            _logger = logger;
        }

        /// <summary>
        /// Get dynamic questions by major per SRS 3.1.1
        /// </summary>
        public async Task<List<AssessmentQuestionDto>> GetQuestionsByMajorAsync(string major)
        {
            _logger.LogInformation($"?? Retrieving assessment questions for major: {major}");

            var questions = await _assessmentRepo.GetQuestionsByMajorAsync(major);

            // Include general questions (null major) as well
            var generalQuestions = await _assessmentRepo.GetAllActiveQuestionsAsync();
            var allQuestions = questions
                .Concat(generalQuestions.Where(q => q.TargetMajor == null))
                .DistinctBy(q => q.Id)
                .OrderBy(q => q.OrderIndex)
                .ToList();

            return allQuestions.Select(MapToDto).ToList();
        }

        /// <summary>
        /// Start new assessment attempt per SRS 3.1
        /// </summary>
        public async Task<AssessmentAttemptDto> StartAssessmentAsync(Guid userId, string major, string studyLevel)
        {
            _logger.LogInformation($"?? Starting new assessment for user {userId}");

            // Check for existing draft attempt
            var existingAttempt = await _assessmentRepo.GetActiveAttemptAsync(userId);
            if (existingAttempt != null && existingAttempt.Status == AssessmentStatus.Draft)
            {
                _logger.LogInformation("?? Found existing draft attempt, returning it");
                return MapAttemptToDto(existingAttempt);
            }

            // Create new attempt
            var attempt = new AssessmentAttempt
            {
                UserId = userId,
                Major = major,
                StudyLevel = Enum.Parse<StudyLevel>(studyLevel, true),
                Status = AssessmentStatus.Draft,
                StartedAt = DateTime.UtcNow
            };

            await _assessmentRepo.CreateAttemptAsync(attempt);
            _logger.LogInformation($"? Assessment attempt {attempt.Id} created");

            return MapAttemptToDto(attempt);
        }

        /// <summary>
        /// Submit single response per SRS 3.1.2
        /// </summary>
        public async Task<AssessmentCompletionResponseDto> SubmitResponseAsync(
            Guid userId,
            Guid attemptId,
            SubmitAssessmentResponseDto responseDto)
        {
            var attempt = await _assessmentRepo.GetAttemptByIdAsync(attemptId, includeResponses: true);
            if (attempt == null || attempt.UserId != userId)
                throw new UnauthorizedAccessException("Invalid assessment attempt");

            // Check for existing response
            var existingResponse = await _assessmentRepo.GetResponseAsync(attemptId, responseDto.QuestionId);
            if (existingResponse != null)
            {
                // Update existing
                existingResponse.ResponseValue = responseDto.ResponseValue;
                existingResponse.IsSkipped = responseDto.IsSkipped;
                existingResponse.Notes = responseDto.Notes;
                existingResponse.UpdatedAt = DateTime.UtcNow;
                await _assessmentRepo.UpdateResponseAsync(existingResponse);
            }
            else
            {
                // Create new response
                var response = new AssessmentResponse
                {
                    UserId = userId,
                    AssessmentAttemptId = attemptId,
                    QuestionId = responseDto.QuestionId,
                    ResponseValue = responseDto.ResponseValue,
                    IsSkipped = responseDto.IsSkipped,
                    Notes = responseDto.Notes
                };
                await _assessmentRepo.CreateResponseAsync(response);
            }

            // Update attempt progress
            await UpdateAttemptProgress(attempt);

            return await BuildCompletionResponse(attempt);
        }

        /// <summary>
        /// Bulk submit responses per SRS 3.1.2
        /// </summary>
        public async Task<AssessmentCompletionResponseDto> BulkSubmitResponsesAsync(
            Guid userId,
            BulkAssessmentSubmissionDto submission)
        {
            var attempt = await _assessmentRepo.GetAttemptByIdAsync(submission.AssessmentAttemptId, includeResponses: true);
            if (attempt == null || attempt.UserId != userId)
                throw new UnauthorizedAccessException("Invalid assessment attempt");

            var responses = submission.Responses.Select(r => new AssessmentResponse
            {
                UserId = userId,
                AssessmentAttemptId = submission.AssessmentAttemptId,
                QuestionId = r.QuestionId,
                ResponseValue = r.ResponseValue,
                IsSkipped = r.IsSkipped,
                Notes = r.Notes
            }).ToList();

            await _assessmentRepo.BulkCreateResponsesAsync(responses);

            // Update attempt progress
            await UpdateAttemptProgress(attempt);

            return await BuildCompletionResponse(attempt);
        }

        /// <summary>
        /// Complete assessment and generate context per SRS 3.1.2
        /// </summary>
        public async Task<AssessmentCompletionResponseDto> CompleteAssessmentAsync(Guid userId, Guid attemptId)
        {
            var attempt = await _assessmentRepo.GetAttemptByIdAsync(attemptId, includeResponses: true);
            if (attempt == null || attempt.UserId != userId)
                throw new UnauthorizedAccessException("Invalid assessment attempt");

            // Build context per SRS 3.1.2
            var context = _contextBuilder.BuildContext(attempt, attempt.Responses.ToList());
            var contextJson = _contextBuilder.SerializeContext(context);

            // Update attempt
            attempt.Status = AssessmentStatus.Completed;
            attempt.CompletedAt = DateTime.UtcNow;
            attempt.ContextJson = contextJson;
            attempt.TotalTimeMinutes = (int)(DateTime.UtcNow - attempt.StartedAt).TotalMinutes;

            await _assessmentRepo.UpdateAttemptAsync(attempt);

            _logger.LogInformation($"? Assessment {attemptId} completed and context generated");

            return await BuildCompletionResponse(attempt);
        }

        /// <summary>
        /// Get serialized assessment context per SRS 3.1.2
        /// </summary>
        public async Task<AssessmentContextDto?> GetAssessmentContextAsync(Guid attemptId)
        {
            var attempt = await _assessmentRepo.GetAttemptByIdAsync(attemptId);
            if (attempt == null || string.IsNullOrWhiteSpace(attempt.ContextJson))
                return null;

            return _contextBuilder.DeserializeContext(attempt.ContextJson);
        }

        // Helper methods

        private async Task UpdateAttemptProgress(AssessmentAttempt attempt)
        {
            var questions = await _assessmentRepo.GetQuestionsByMajorAsync(attempt.Major);
            var totalQuestions = questions.Count(q => q.IsRequired);
            var answeredQuestions = attempt.Responses.Count(r => !r.IsSkipped);

            attempt.CompletionPercentage = totalQuestions > 0
                ? (int)((double)answeredQuestions / totalQuestions * 100)
                : 0;

            await _assessmentRepo.UpdateAttemptAsync(attempt);
        }

        private async Task<AssessmentCompletionResponseDto> BuildCompletionResponse(AssessmentAttempt attempt)
        {
            var questions = await _assessmentRepo.GetQuestionsByMajorAsync(attempt.Major);
            var totalQuestions = questions.Count;
            var answeredQuestions = attempt.Responses.Count(r => !r.IsSkipped);

            return new AssessmentCompletionResponseDto
            {
                AssessmentAttemptId = attempt.Id,
                Status = attempt.Status.ToString(),
                TotalQuestions = totalQuestions,
                AnsweredQuestions = answeredQuestions,
                CompletionPercentage = attempt.CompletionPercentage,
                ContextJson = attempt.ContextJson,
                CanGenerateStudyPlan = attempt.Status == AssessmentStatus.Completed
            };
        }

        private AssessmentQuestionDto MapToDto(AssessmentQuestion question)
        {
            return new AssessmentQuestionDto
            {
                Id = question.Id,
                QuestionText = question.QuestionText,
                QuestionType = question.QuestionType.ToString(),
                Category = question.Category,
                OrderIndex = question.OrderIndex,
                IsRequired = question.IsRequired,
                Options = ParseOptions(question.OptionsJson),
                MinValue = question.MinValue,
                MaxValue = question.MaxValue,
                HelpText = question.HelpText,
                ValidationPattern = question.ValidationPattern
            };
        }

        private List<string>? ParseOptions(string? optionsJson)
        {
            if (string.IsNullOrWhiteSpace(optionsJson))
                return null;

            try
            {
                return JsonSerializer.Deserialize<List<string>>(optionsJson);
            }
            catch
            {
                return null;
            }
        }

        private AssessmentAttemptDto MapAttemptToDto(AssessmentAttempt attempt)
        {
            return new AssessmentAttemptDto
            {
                Id = attempt.Id,
                Major = attempt.Major,
                StudyLevel = attempt.StudyLevel.ToString(),
                Status = attempt.Status.ToString(),
                StartedAt = attempt.StartedAt,
                CompletedAt = attempt.CompletedAt,
                CompletionPercentage = attempt.CompletionPercentage
            };
        }
    }
}
