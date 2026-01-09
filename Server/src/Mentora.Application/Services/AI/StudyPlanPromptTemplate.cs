using Mentora.Application.DTOs.Assessment;
using System.Text.Json;

namespace Mentora.Application.Services.AI
{
    /// <summary>
    /// Service for constructing AI prompts with rigid JSON schema per SRS 3.2.1
    /// Enforces structured output for study plan generation
    /// </summary>
    public class StudyPlanPromptTemplate
    {
        private const string SystemPromptTemplate = @"You are an expert academic advisor and learning strategist. Your task is to create a comprehensive, personalized study plan based on the student's profile and assessment responses.

STRICT OUTPUT REQUIREMENTS:
- You MUST respond with ONLY valid JSON
- Follow the exact schema provided below
- Do not include any text outside the JSON structure
- Ensure all required fields are populated

JSON Schema:
{
  ""title"": ""string (50-200 chars)"",
  ""summary"": ""string (200-500 chars)"",
  ""estimatedHours"": number,
  ""difficultyLevel"": ""Beginner|Intermediate|Advanced|Expert"",
  ""steps"": [
    {
      ""name"": ""string (30-100 chars)"",
      ""description"": ""string (100-500 chars)"",
      ""orderIndex"": number,
      ""estimatedHours"": number,
      ""objectives"": [""string""],
      ""checkpoints"": [
        {
          ""description"": ""string (20-200 chars)"",
          ""orderIndex"": number,
          ""estimatedMinutes"": number,
          ""type"": ""Setup|Reading|Exercise|Project|Practice|Assessment"",
          ""isMandatory"": boolean
        }
      ]
    }
  ],
  ""requiredSkills"": [
    {
      ""skillName"": ""string"",
      ""targetProficiency"": ""Beginner|Intermediate|Advanced"",
      ""importance"": number (1-5),
      ""isPrerequisite"": boolean
    }
  ],
  ""resources"": [
    {
      ""title"": ""string"",
      ""url"": ""string (valid URL)"",
      ""resourceType"": ""Course|Article|Video|Book|Documentation|Tutorial|Practice|Project"",
      ""description"": ""string (50-300 chars)"",
      ""estimatedHours"": number,
      ""difficultyLevel"": ""Beginner|Intermediate|Advanced|Expert"",
      ""isFree"": boolean,
      ""cost"": number (if not free),
      ""provider"": ""string"",
      ""priority"": number (1-5)
    }
  ]
}

REQUIREMENTS:
1. Plan must be practical and achievable within the given timeframe
2. Steps must be ordered logically with clear progression
3. Each step should have 3-8 checkpoints
4. Include mix of free and paid resources (prefer free when possible)
5. Skills should be realistic for the student's level
6. Total estimated hours should align with weekly availability * weeks until goal
7. Resources must be real, verifiable, and currently available
8. Include diverse resource types (courses, videos, articles, practice)
9. Checkpoints should be specific, actionable micro-tasks
10. Difficulty should match the student's current level and goals";

        /// <summary>
        /// Build complete AI prompt with context injection per SRS 3.2.1
        /// </summary>
        public string BuildPrompt(AssessmentContextDto context, string? additionalInstructions = null)
        {
            var userPrompt = $@"{BuildContextSection(context)}

{BuildConstraintsSection(context)}

{BuildGoalsSection(context)}

{(string.IsNullOrWhiteSpace(additionalInstructions) ? "" : $"\nADDITIONAL INSTRUCTIONS:\n{additionalInstructions}\n")}

Based on the above information, generate a comprehensive, personalized study plan in the exact JSON format specified in the system prompt. Focus on practical, achievable steps that will help the student reach their goals within the given constraints.";

            return userPrompt;
        }

        /// <summary>
        /// Get system prompt
        /// </summary>
        public string GetSystemPrompt() => SystemPromptTemplate;

        /// <summary>
        /// Build context section from assessment data
        /// </summary>
        private string BuildContextSection(AssessmentContextDto context)
        {
            return $@"STUDENT CONTEXT:
Major: {context.Major}
Study Level: {context.StudyLevel}
Current Skills: {(context.CurrentSkills.Any() ? string.Join(", ", context.CurrentSkills) : "None specified")}
Areas of Interest: {(context.InterestedAreas.Any() ? string.Join(", ", context.InterestedAreas) : "General")}
Career Goal: {(string.IsNullOrWhiteSpace(context.CareerGoal) ? "Not specified" : context.CareerGoal)}
Learning Style: {(string.IsNullOrWhiteSpace(context.LearningStyle) ? "Flexible" : context.LearningStyle)}";
        }

        /// <summary>
        /// Build constraints section per SRS 3.2.1
        /// </summary>
        private string BuildConstraintsSection(AssessmentContextDto context)
        {
            var constraints = new List<string>();

            if (context.YearsUntilGraduation > 0)
                constraints.Add($"- Time until graduation: {context.YearsUntilGraduation} years");

            if (context.WeeklyHoursAvailable > 0)
                constraints.Add($"- Weekly study time available: {context.WeeklyHoursAvailable} hours");

            // Calculate total available hours if both constraints exist
            if (context.YearsUntilGraduation > 0 && context.WeeklyHoursAvailable > 0)
            {
                var weeksPerYear = 48; // Account for breaks
                var totalWeeks = context.YearsUntilGraduation * weeksPerYear;
                var totalHours = totalWeeks * context.WeeklyHoursAvailable;
                constraints.Add($"- Total available study hours: ~{totalHours} hours");
            }

            return constraints.Any() 
                ? $"CONSTRAINTS:\n{string.Join("\n", constraints)}"
                : "CONSTRAINTS: Flexible timeline";
        }

        /// <summary>
        /// Build goals section
        /// </summary>
        private string BuildGoalsSection(AssessmentContextDto context)
        {
            var goals = new List<string>();

            if (!string.IsNullOrWhiteSpace(context.CareerGoal))
                goals.Add($"- Primary Career Goal: {context.CareerGoal}");

            if (context.InterestedAreas.Any())
                goals.Add($"- Areas to Develop: {string.Join(", ", context.InterestedAreas)}");

            // Add implicit goals based on context
            if (context.YearsUntilGraduation <= 1)
                goals.Add("- Prepare for job market entry");
            else if (context.YearsUntilGraduation <= 2)
                goals.Add("- Build strong foundation for internship opportunities");

            return goals.Any()
                ? $"LEARNING GOALS:\n{string.Join("\n", goals)}"
                : "LEARNING GOALS: Comprehensive skill development";
        }

        /// <summary>
        /// Validate AI response structure per SRS 3.2.2
        /// </summary>
        public bool ValidateResponse(string aiResponse, out string? error)
        {
            error = null;

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                using var document = JsonDocument.Parse(aiResponse);
                var root = document.RootElement;

                // Validate required fields
                if (!root.TryGetProperty("title", out _))
                {
                    error = "Missing required field: title";
                    return false;
                }

                if (!root.TryGetProperty("summary", out _))
                {
                    error = "Missing required field: summary";
                    return false;
                }

                if (!root.TryGetProperty("steps", out var steps) || steps.GetArrayLength() == 0)
                {
                    error = "Missing or empty required field: steps";
                    return false;
                }

                // Validate at least one step has checkpoints
                var hasCheckpoints = false;
                foreach (var step in steps.EnumerateArray())
                {
                    if (step.TryGetProperty("checkpoints", out var checkpoints) && 
                        checkpoints.GetArrayLength() > 0)
                    {
                        hasCheckpoints = true;
                        break;
                    }
                }

                if (!hasCheckpoints)
                {
                    error = "At least one step must have checkpoints";
                    return false;
                }

                return true;
            }
            catch (JsonException ex)
            {
                error = $"Invalid JSON format: {ex.Message}";
                return false;
            }
        }
    }
}
