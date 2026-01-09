using Mentora.Application.DTOs.Assessment;
using Mentora.Application.Interfaces.Services;
using Mentora.Application.Services.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace Mentora.Application.Services
{
    /// <summary>
    /// Google Gemini AI Service for Study Plan Generation per SRS Section 3 & 4
    /// Uses Gemini 2.0 Flash for intelligent, personalized study plan creation
    /// </summary>
    public class GeminiAiStudyPlanService : IAiStudyPlanService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GeminiAiStudyPlanService> _logger;
        private readonly StudyPlanPromptTemplate _promptTemplate;
        private readonly string _apiKey;
        private readonly string _model;
        private readonly double _temperature;

        public GeminiAiStudyPlanService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<GeminiAiStudyPlanService> logger,
            StudyPlanPromptTemplate promptTemplate)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
            _logger = logger;
            _promptTemplate = promptTemplate;

            // Load configuration
            _apiKey = _configuration["GoogleAI:ApiKey"]
                ?? throw new InvalidOperationException("Google AI API Key not configured");
            _model = _configuration["GoogleAI:Model"] ?? "gemini-2.0-flash";
            _temperature = double.Parse(_configuration["GoogleAI:Temperature"] ?? "0.7");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.Timeout = TimeSpan.FromSeconds(60);
        }

        /// <summary>
        /// Generate study plan using Gemini AI per SRS 3.2
        /// </summary>
        public async Task<AiStudyPlanResponse> GenerateStudyPlanAsync(
            AssessmentContextDto context,
            string? additionalInstructions = null)
        {
            try
            {
                _logger.LogInformation("?? Starting Gemini AI Study Plan Generation...");
                _logger.LogInformation($"?? Major: {context.Major}, Level: {context.StudyLevel}");

                // 1. Build prompt with context injection per SRS 3.2.1
                var systemPrompt = _promptTemplate.GetSystemPrompt();
                var userPrompt = _promptTemplate.BuildPrompt(context, additionalInstructions);

                // 2. Call Gemini API
                _logger.LogInformation("?? Calling Gemini API...");
                var geminiResponse = await CallGeminiAsync(systemPrompt, userPrompt);

                // 3. Validate and parse response per SRS 3.2.2
                _logger.LogInformation("? Parsing AI response...");
                if (!ValidateResponse(geminiResponse, out var error))
                {
                    _logger.LogWarning($"?? AI response validation warning: {error}");
                    // Continue with parsing, validation is informational
                }

                var studyPlan = ParseResponse(geminiResponse);
                studyPlan.AiModel = _model;
                studyPlan.RawResponse = geminiResponse;

                _logger.LogInformation($"?? Study plan generated successfully! {studyPlan.Steps.Count} steps, {studyPlan.RequiredSkills.Count} skills");
                return studyPlan;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "? Gemini API request failed");
                throw new InvalidOperationException("AI service unavailable. Please try again later.", ex);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "? Failed to parse Gemini response");
                throw new InvalidOperationException("AI returned invalid response format.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Unexpected error in AI generation");
                throw;
            }
        }

        /// <summary>
        /// Validate AI response structure per SRS 3.2.2
        /// </summary>
        public bool ValidateResponse(string aiResponse, out string? error)
        {
            return _promptTemplate.ValidateResponse(aiResponse, out error);
        }

        /// <summary>
        /// Call Gemini API with system and user prompts
        /// </summary>
        private async Task<string> CallGeminiAsync(string systemPrompt, string userPrompt)
        {
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = new[]
                        {
                            new { text = systemPrompt + "\n\n" + userPrompt }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = _temperature,
                    maxOutputTokens = 8192, // Larger for comprehensive study plans
                    responseMimeType = "application/json" // Force JSON response
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/{_model}:generateContent?key={_apiKey}";
            var response = await _httpClient.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Gemini API Error: {response.StatusCode} - {errorContent}");
                throw new HttpRequestException($"Gemini API returned {response.StatusCode}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(responseBody);

            var aiContent = geminiResponse?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;
            if (string.IsNullOrEmpty(aiContent))
            {
                throw new InvalidOperationException("Gemini returned empty response");
            }

            return aiContent;
        }

        /// <summary>
        /// Parse and map AI response to domain model per SRS 3.2.2
        /// </summary>
        private AiStudyPlanResponse ParseResponse(string jsonResponse)
        {
            // Clean response
            jsonResponse = CleanJsonResponse(jsonResponse);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true
            };

            var response = JsonSerializer.Deserialize<AiStudyPlanResponse>(jsonResponse, options);
            if (response == null)
            {
                throw new JsonException("Failed to deserialize AI response");
            }

            response.GeneratedAt = DateTime.UtcNow;

            // Validate structure
            ValidateStudyPlanStructure(response);

            return response;
        }

        /// <summary>
        /// Clean JSON response from markdown formatting
        /// </summary>
        private string CleanJsonResponse(string response)
        {
            response = response.Trim();

            if (response.StartsWith("```json"))
                response = response.Substring(7);
            else if (response.StartsWith("```"))
                response = response.Substring(3);

            if (response.EndsWith("```"))
                response = response.Substring(0, response.Length - 3);

            return response.Trim();
        }

        /// <summary>
        /// Validate study plan structure per SRS 3.2.2
        /// </summary>
        private void ValidateStudyPlanStructure(AiStudyPlanResponse response)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(response.Title))
                errors.Add("Title is missing");

            if (string.IsNullOrWhiteSpace(response.Summary))
                errors.Add("Summary is missing");

            if (response.Steps == null || !response.Steps.Any())
                errors.Add("No steps provided");

            if (response.Steps != null)
            {
                var stepsWithoutCheckpoints = response.Steps.Where(s => s.Checkpoints == null || !s.Checkpoints.Any()).ToList();
                if (stepsWithoutCheckpoints.Any())
                    errors.Add($"{stepsWithoutCheckpoints.Count} steps have no checkpoints");
            }

            if (response.RequiredSkills == null || !response.RequiredSkills.Any())
                errors.Add("No required skills provided");

            if (response.Resources == null || !response.Resources.Any())
                errors.Add("No learning resources provided");

            if (errors.Any())
            {
                var errorMessage = $"AI response validation failed:\n{string.Join("\n", errors)}";
                _logger.LogWarning(errorMessage);
                // Don't throw - allow incomplete plans to be saved for manual editing
            }
            else
            {
                _logger.LogInformation("? AI response validated successfully");
            }
        }

        // Gemini API Response Classes
        private class GeminiResponse
        {
            public List<Candidate>? Candidates { get; set; }
        }

        private class Candidate
        {
            public ContentResponse? Content { get; set; }
        }

        private class ContentResponse
        {
            public List<Part>? Parts { get; set; }
        }

        private class Part
        {
            public string? Text { get; set; }
        }
    }
}
