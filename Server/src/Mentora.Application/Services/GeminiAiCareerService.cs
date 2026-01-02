using Mentora.Application.Interfaces.Services;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Mentora.Application.Services
{
    /// <summary>
    /// Google Gemini AI Service for Career Plan Generation
    /// Uses Google's Gemini 2.0 Flash model for fast, intelligent career planning
    /// Per SRS Section 7: AI Integration Rules
    /// </summary>
    public class GeminiAiCareerService : IAiCareerService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GeminiAiCareerService> _logger;
        private readonly string _apiKey;
        private readonly string _model;
        private readonly double _temperature;

        public GeminiAiCareerService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<GeminiAiCareerService> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
            _logger = logger;

            // Load configuration
            _apiKey = _configuration["GoogleAI:ApiKey"] 
                ?? throw new InvalidOperationException("Google AI API Key not configured in appsettings.json");
            _model = _configuration["GoogleAI:Model"] ?? "gemini-2.0-flash";
            _temperature = double.Parse(_configuration["GoogleAI:Temperature"] ?? "0.7");

            // Configure HttpClient
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.Timeout = TimeSpan.FromSeconds(60);
        }

        public async Task<AiCareerPlanResponse> GenerateCareerPlanAsync(string quizAnswersJson)
        {
            try
            {
                _logger.LogInformation("?? Starting Google Gemini AI Career Plan Generation...");

                // 1. Parse quiz answers
                var answers = JsonSerializer.Deserialize<List<QuizAnswer>>(quizAnswersJson) 
                    ?? throw new InvalidOperationException("Failed to parse quiz answers");

                // 2. Extract user data
                var userData = ExtractUserData(answers);
                _logger.LogInformation($"?? User Goal: {userData.CareerGoal}, Experience: {userData.Experience}");

                // 3. Build intelligent prompt
                var prompt = BuildIntelligentPrompt(userData);

                // 4. Call Gemini API
                _logger.LogInformation("?? Calling Google Gemini API...");
                var geminiResponse = await CallGeminiAsync(prompt);

                // 5. Parse and validate response
                _logger.LogInformation("? Parsing AI response...");
                var careerPlan = ParseAndValidateResponse(geminiResponse, userData);

                _logger.LogInformation("?? Career plan generated successfully with Google Gemini!");
                return careerPlan;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "? Google Gemini API request failed");
                return await FallbackToIntelligentMockAsync(quizAnswersJson);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "? Failed to parse Gemini response");
                return await FallbackToIntelligentMockAsync(quizAnswersJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Unexpected error in AI generation");
                return await FallbackToIntelligentMockAsync(quizAnswersJson);
            }
        }

        private UserCareerData ExtractUserData(List<QuizAnswer> answers)
        {
            return new UserCareerData
            {
                CareerGoal = answers.FirstOrDefault(a => a.QuestionId == "q1")?.Answer 
                    ?? "Software Developer",
                Experience = answers.FirstOrDefault(a => a.QuestionId == "q2")?.Answer 
                    ?? "0-2 years",
                WorkStyle = answers.FirstOrDefault(a => a.QuestionId == "q3")?.Answer 
                    ?? "Not specified",
                SkillsToLearn = answers.FirstOrDefault(a => a.QuestionId == "q4")?.Answer 
                    ?? "Not specified",
                Obstacles = answers.FirstOrDefault(a => a.QuestionId == "q5")?.Answer 
                    ?? "Not specified",
                TimeAvailable = answers.FirstOrDefault(a => a.QuestionId == "q6")?.Answer 
                    ?? "5-10 hours"
            };
        }

        private string BuildIntelligentPrompt(UserCareerData userData)
        {
            return $@"You are an expert career advisor AI with deep knowledge in technology careers, skill development, and professional growth strategies.

**USER PROFILE:**
- Career Goal: {userData.CareerGoal}
- Current Experience: {userData.Experience}
- Preferred Work Style: {userData.WorkStyle}
- Skills to Develop: {userData.SkillsToLearn}
- Main Obstacles: {userData.Obstacles}
- Time Available Weekly: {userData.TimeAvailable}

**YOUR MISSION:**
Generate a comprehensive, actionable, and PERSONALIZED career development plan that:
1. Addresses the user's specific career goal
2. Considers their experience level realistically
3. Tackles their mentioned obstacles
4. Fits their available time commitment
5. Includes both technical AND soft skills
6. Provides progressive, achievable steps

**STRUCTURE REQUIREMENTS:**
- Title: Compelling, specific to their goal (e.g., ""From Junior to Senior {userData.CareerGoal} in 12 Months"")
- Summary: 2-3 sentences explaining the plan's approach and timeline
- Exactly 4 progressive steps:
  * Step 1: Foundation (basics, setup, first skills)
  * Step 2: Core Development (main technical skills)
  * Step 3: Advanced Topics (architecture, patterns, best practices)
  * Step 4: Real-World Application (projects, portfolio, experience)
- 12-16 skills total:
  * 8-12 Technical skills (specific to {userData.CareerGoal})
  * 4-6 Soft skills (communication, problem-solving, etc.)
- Each step should have 3-5 skills assigned

**IMPORTANT RULES:**
? Make it REALISTIC - consider experience level
? Make it SPECIFIC - use actual technologies/tools
? Make it ACTIONABLE - clear, measurable skills
? Make it PROGRESSIVE - build on previous steps
? Address their obstacles creatively
? Skills must be ""Technical"" or ""Soft"" category EXACTLY

**OUTPUT FORMAT (VALID JSON ONLY):**
{{
  ""title"": ""From Junior to {userData.CareerGoal} in X Months"",
  ""summary"": ""A personalized X-month plan that focuses on [specific approach based on user data]..."",
  ""steps"": [
    {{
      ""name"": ""Foundation Phase"",
      ""description"": ""Build your foundational skills and setup your development environment..."",
      ""orderIndex"": 0,
      ""skillNames"": [""Git"", ""Basic Programming"", ""Time Management"", ""Problem Solving""]
    }},
    {{
      ""name"": ""Core Skills Development"",
      ""description"": ""Master the essential technical skills for {userData.CareerGoal}..."",
      ""orderIndex"": 1,
      ""skillNames"": [""C#"", ""ASP.NET Core"", ""SQL"", ""Communication""]
    }},
    {{
      ""name"": ""Advanced Concepts"",
      ""description"": ""Dive into advanced topics and industry best practices..."",
      ""orderIndex"": 2,
      ""skillNames"": [""Design Patterns"", ""Microservices"", ""CI/CD"", ""Leadership""]
    }},
    {{
      ""name"": ""Real-World Application"",
      ""description"": ""Build production-ready projects and gain practical experience..."",
      ""orderIndex"": 3,
      ""skillNames"": [""System Design"", ""Cloud Services"", ""Teamwork""]
    }}
  ],
  ""skills"": [
    {{ ""name"": ""Git"", ""category"": ""Technical"" }},
    {{ ""name"": ""Time Management"", ""category"": ""Soft"" }},
    ...
  ]
}}

**CRITICAL:**
- Return ONLY the JSON object
- No markdown code blocks (```json)
- No explanations or extra text
- Ensure valid JSON syntax
- Skills category must be EXACTLY ""Technical"" or ""Soft""
- Total skills: 12-16
- Each step must have 3-5 skills

Generate the plan now:";
        }

        private async Task<string> CallGeminiAsync(string prompt)
        {
            // Build Gemini API request
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = _temperature,
                    maxOutputTokens = 4096,
                    responseMimeType = "application/json" // Force JSON response
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Gemini API endpoint
            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/{_model}:generateContent?key={_apiKey}";

            var response = await _httpClient.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Google Gemini API Error: {response.StatusCode} - {errorContent}");
                throw new HttpRequestException($"Google Gemini API returned {response.StatusCode}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(responseBody);

            var aiContent = geminiResponse?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;
            if (string.IsNullOrEmpty(aiContent))
            {
                throw new InvalidOperationException("Google Gemini returned empty response");
            }

            return aiContent;
        }

        private AiCareerPlanResponse ParseAndValidateResponse(string jsonResponse, UserCareerData userData)
        {
            // Clean up response (remove markdown if present)
            jsonResponse = jsonResponse.Trim();
            if (jsonResponse.StartsWith("```json"))
            {
                jsonResponse = jsonResponse.Substring(7);
            }
            if (jsonResponse.StartsWith("```"))
            {
                jsonResponse = jsonResponse.Substring(3);
            }
            if (jsonResponse.EndsWith("```"))
            {
                jsonResponse = jsonResponse.Substring(0, jsonResponse.Length - 3);
            }
            jsonResponse = jsonResponse.Trim();

            // Parse JSON
            var options = new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true
            };
            
            var response = JsonSerializer.Deserialize<AiCareerPlanResponse>(jsonResponse, options);

            if (response == null)
            {
                throw new JsonException("Failed to deserialize AI response");
            }

            // Validate structure
            ValidateCareerPlanStructure(response);

            // Enhance with metadata
            response.GeneratedAt = DateTime.UtcNow;
            response.AiModel = _model;

            return response;
        }

        private void ValidateCareerPlanStructure(AiCareerPlanResponse response)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(response.Title))
                errors.Add("Title is missing");

            if (string.IsNullOrWhiteSpace(response.Summary))
                errors.Add("Summary is missing");

            if (response.Steps == null || response.Steps.Count != 4)
                errors.Add($"Must have exactly 4 steps, got {response.Steps?.Count ?? 0}");

            if (response.Skills == null || response.Skills.Count < 12 || response.Skills.Count > 16)
                errors.Add($"Must have 12-16 skills, got {response.Skills?.Count ?? 0}");

            // Validate skill categories
            if (response.Skills != null)
            {
                var invalidSkills = response.Skills
                    .Where(s => s.Category != "Technical" && s.Category != "Soft")
                    .ToList();

                if (invalidSkills.Any())
                {
                    errors.Add($"Invalid skill categories found: {string.Join(", ", invalidSkills.Select(s => s.Name))}");
                }
            }

            // Validate steps have skills
            if (response.Steps != null)
            {
                foreach (var step in response.Steps)
                {
                    if (step.SkillNames == null || !step.SkillNames.Any())
                    {
                        errors.Add($"Step '{step.Name}' has no skills assigned");
                    }
                }
            }

            if (errors.Any())
            {
                var errorMessage = $"AI response validation failed:\n{string.Join("\n", errors)}";
                _logger.LogError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            _logger.LogInformation("? AI response validated successfully");
        }

        private async Task<AiCareerPlanResponse> FallbackToIntelligentMockAsync(string quizAnswersJson)
        {
            _logger.LogWarning("??  Using intelligent mock fallback...");

            // Parse answers for personalization even in fallback
            var answers = JsonSerializer.Deserialize<List<QuizAnswer>>(quizAnswersJson) ?? new List<QuizAnswer>();
            var userData = ExtractUserData(answers);

            await Task.Delay(500); // Simulate processing

            return new AiCareerPlanResponse
            {
                Title = $"Career Path to {userData.CareerGoal}",
                Summary = $"A personalized {GetTimelineFromExperience(userData.Experience)} plan tailored to your goal of becoming a {userData.CareerGoal}. This plan considers your {userData.Experience} experience level and available {userData.TimeAvailable} weekly commitment.",
                Steps = GenerateIntelligentSteps(userData),
                Skills = GenerateIntelligentSkills(userData),
                GeneratedAt = DateTime.UtcNow,
                AiModel = "fallback-intelligent-mock"
            };
        }

        private string GetTimelineFromExperience(string experience)
        {
            return experience switch
            {
                "0-2 years" => "12-month",
                "3-5 years" => "9-month",
                "5-10 years" => "6-month",
                "10+ years" => "4-month",
                _ => "12-month"
            };
        }

        private List<AiCareerStep> GenerateIntelligentSteps(UserCareerData userData)
        {
            var isExperienced = userData.Experience.Contains("5-10") || userData.Experience.Contains("10+");

            return new List<AiCareerStep>
            {
                new AiCareerStep
                {
                    Name = isExperienced ? "Advanced Foundation" : "Foundation Phase",
                    Description = isExperienced 
                        ? $"Refresh fundamentals and prepare for {userData.CareerGoal} transition."
                        : "Build strong fundamentals and establish your learning routine.",
                    OrderIndex = 0,
                    SkillNames = new List<string> { "Git", "Programming Basics", "Time Management", "Problem Solving" }
                },
                new AiCareerStep
                {
                    Name = "Core Skills Development",
                    Description = $"Master essential technical skills for {userData.CareerGoal}.",
                    OrderIndex = 1,
                    SkillNames = new List<string> { "C#", "ASP.NET Core", "SQL", "Communication" }
                },
                new AiCareerStep
                {
                    Name = "Advanced Concepts & Best Practices",
                    Description = "Dive into advanced topics, design patterns, and industry standards.",
                    OrderIndex = 2,
                    SkillNames = new List<string> { "Design Patterns", "Microservices", "CI/CD", "Leadership" }
                },
                new AiCareerStep
                {
                    Name = "Real-World Projects & Portfolio",
                    Description = "Apply knowledge through production-ready projects and build your portfolio.",
                    OrderIndex = 3,
                    SkillNames = new List<string> { "System Design", "Cloud Services", "Teamwork" }
                }
            };
        }

        private List<AiCareerSkill> GenerateIntelligentSkills(UserCareerData userData)
        {
            return new List<AiCareerSkill>
            {
                // Technical Skills (12)
                new AiCareerSkill { Name = "Git", Category = "Technical" },
                new AiCareerSkill { Name = "Programming Basics", Category = "Technical" },
                new AiCareerSkill { Name = "C#", Category = "Technical" },
                new AiCareerSkill { Name = "ASP.NET Core", Category = "Technical" },
                new AiCareerSkill { Name = "SQL", Category = "Technical" },
                new AiCareerSkill { Name = "Design Patterns", Category = "Technical" },
                new AiCareerSkill { Name = "Microservices", Category = "Technical" },
                new AiCareerSkill { Name = "CI/CD", Category = "Technical" },
                new AiCareerSkill { Name = "System Design", Category = "Technical" },
                new AiCareerSkill { Name = "Cloud Services", Category = "Technical" },
                new AiCareerSkill { Name = "Docker", Category = "Technical" },
                new AiCareerSkill { Name = "REST APIs", Category = "Technical" },

                // Soft Skills (4)
                new AiCareerSkill { Name = "Time Management", Category = "Soft" },
                new AiCareerSkill { Name = "Problem Solving", Category = "Soft" },
                new AiCareerSkill { Name = "Communication", Category = "Soft" },
                new AiCareerSkill { Name = "Leadership", Category = "Soft" }
            };
        }

        // Helper classes
        private class QuizAnswer
        {
            public string QuestionId { get; set; } = string.Empty;
            public string Answer { get; set; } = string.Empty;
        }

        private class UserCareerData
        {
            public string CareerGoal { get; set; } = string.Empty;
            public string Experience { get; set; } = string.Empty;
            public string WorkStyle { get; set; } = string.Empty;
            public string SkillsToLearn { get; set; } = string.Empty;
            public string Obstacles { get; set; } = string.Empty;
            public string TimeAvailable { get; set; } = string.Empty;
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
