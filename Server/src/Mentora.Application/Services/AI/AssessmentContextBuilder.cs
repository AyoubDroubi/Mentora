using Mentora.Application.DTOs.Assessment;
using Mentora.Domain.Entities.Assessment;
using System.Text.Json;

namespace Mentora.Application.Services.AI
{
    /// <summary>
    /// Service for building assessment context for AI injection per SRS 3.1.2
    /// Serializes user responses into simplified JSON format
    /// </summary>
    public class AssessmentContextBuilder
    {
        /// <summary>
        /// Build AI context from assessment attempt and responses per SRS 3.1.2
        /// </summary>
        public AssessmentContextDto BuildContext(AssessmentAttempt attempt, List<AssessmentResponse> responses)
        {
            var context = new AssessmentContextDto
            {
                Major = attempt.Major,
                StudyLevel = attempt.StudyLevel.ToString()
            };

            // Extract key information from responses
            foreach (var response in responses.Where(r => !r.IsSkipped))
            {
                var questionCategory = response.Question.Category.ToLowerInvariant();
                var value = response.ResponseValue;

                switch (questionCategory)
                {
                    case "timeline":
                    case "graduation":
                        if (int.TryParse(value, out int years))
                            context.YearsUntilGraduation = years;
                        break;

                    case "skills":
                    case "current skills":
                        context.CurrentSkills.AddRange(ParseListResponse(value));
                        break;

                    case "interests":
                    case "focus areas":
                        context.InterestedAreas.AddRange(ParseListResponse(value));
                        break;

                    case "career goal":
                    case "goal":
                        context.CareerGoal = value;
                        break;

                    case "availability":
                    case "time commitment":
                        if (int.TryParse(value, out int hours))
                            context.WeeklyHoursAvailable = hours;
                        break;

                    case "learning style":
                    case "preferences":
                        context.LearningStyle = value;
                        break;

                    default:
                        // Store other responses in additional context
                        var key = response.Question.QuestionText
                            .Replace("?", "")
                            .Replace(" ", "_")
                            .ToLowerInvariant()
                            .Substring(0, Math.Min(50, response.Question.QuestionText.Length));
                        context.AdditionalContext[key] = value;
                        break;
                }
            }

            return context;
        }

        /// <summary>
        /// Serialize context to JSON string for storage per SRS 3.1.2
        /// </summary>
        public string SerializeContext(AssessmentContextDto context)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            return JsonSerializer.Serialize(context, options);
        }

        /// <summary>
        /// Deserialize context from JSON string
        /// </summary>
        public AssessmentContextDto? DeserializeContext(string contextJson)
        {
            if (string.IsNullOrWhiteSpace(contextJson))
                return null;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            return JsonSerializer.Deserialize<AssessmentContextDto>(contextJson, options);
        }

        /// <summary>
        /// Build simplified prompt-ready string from context per SRS 3.2.1
        /// </summary>
        public string BuildPromptContext(AssessmentContextDto context)
        {
            var promptParts = new List<string>();

            promptParts.Add($"Student Profile:");
            promptParts.Add($"- Major: {context.Major}");
            promptParts.Add($"- Study Level: {context.StudyLevel}");
            
            if (context.YearsUntilGraduation > 0)
                promptParts.Add($"- Time Until Graduation: {context.YearsUntilGraduation} years");

            if (context.CurrentSkills.Any())
                promptParts.Add($"- Current Skills: {string.Join(", ", context.CurrentSkills)}");

            if (context.InterestedAreas.Any())
                promptParts.Add($"- Areas of Interest: {string.Join(", ", context.InterestedAreas)}");

            if (!string.IsNullOrWhiteSpace(context.CareerGoal))
                promptParts.Add($"- Career Goal: {context.CareerGoal}");

            if (context.WeeklyHoursAvailable > 0)
                promptParts.Add($"- Weekly Study Time Available: {context.WeeklyHoursAvailable} hours");

            if (!string.IsNullOrWhiteSpace(context.LearningStyle))
                promptParts.Add($"- Learning Style: {context.LearningStyle}");

            // Add additional context if available
            if (context.AdditionalContext.Any())
            {
                promptParts.Add("\nAdditional Context:");
                foreach (var kvp in context.AdditionalContext.Take(5)) // Limit to top 5
                {
                    promptParts.Add($"- {kvp.Key}: {kvp.Value}");
                }
            }

            return string.Join("\n", promptParts);
        }

        /// <summary>
        /// Helper to parse list responses (comma or newline separated)
        /// </summary>
        private List<string> ParseListResponse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return new List<string>();

            // Try JSON array first
            try
            {
                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var list = JsonSerializer.Deserialize<List<string>>(value, jsonOptions);
                if (list != null && list.Any())
                    return list.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            }
            catch
            {
                // Not JSON, try delimiter parsing
            }

            // Try comma or newline separated
            var delimiters = new[] { ',', '\n', ';' };
            return value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();
        }
    }
}
