using Mentora.Application.Interfaces.Services;
using System.Text.Json;

namespace Mentora.Application.Services
{
    /// <summary>
    /// Enhanced Mock AI Service for Career Plan Generation
    /// Provides intelligent fallback when OpenAI is unavailable
    /// Per SRS Section 7: AI Integration Rules
    /// </summary>
    public class MockAiCareerService : IAiCareerService
    {
        public async Task<AiCareerPlanResponse> GenerateCareerPlanAsync(string quizAnswersJson)
        {
            // Simulate AI processing delay
            await Task.Delay(1500);

            // Parse quiz answers to personalize response
            var answers = JsonSerializer.Deserialize<List<QuizAnswer>>(quizAnswersJson) ?? new List<QuizAnswer>();
            
            var careerGoal = answers.FirstOrDefault(a => a.QuestionId == "q1")?.Answer ?? "Software Developer";
            var experience = answers.FirstOrDefault(a => a.QuestionId == "q2")?.Answer ?? "0-2 years";
            var workStyle = answers.FirstOrDefault(a => a.QuestionId == "q3")?.Answer ?? "Not specified";
            var skillsToLearn = answers.FirstOrDefault(a => a.QuestionId == "q4")?.Answer ?? "";
            var timeAvailable = answers.FirstOrDefault(a => a.QuestionId == "q6")?.Answer ?? "5-10 hours";

            // Generate personalized mock response
            return new AiCareerPlanResponse
            {
                Title = GeneratePersonalizedTitle(careerGoal, experience),
                Summary = GeneratePersonalizedSummary(careerGoal, experience, workStyle, timeAvailable),
                Steps = GenerateIntelligentSteps(careerGoal, experience, skillsToLearn),
                Skills = GenerateContextualSkills(careerGoal, skillsToLearn),
                GeneratedAt = DateTime.UtcNow,
                AiModel = "intelligent-mock-v2"
            };
        }

        private string GeneratePersonalizedTitle(string careerGoal, string experience)
        {
            var timeline = GetTimelineFromExperience(experience);
            var level = GetLevelFromExperience(experience);
            
            return $"{level} Path to {careerGoal} - {timeline} Journey";
        }

        private string GeneratePersonalizedSummary(string careerGoal, string experience, string workStyle, string timeAvailable)
        {
            var timeline = GetTimelineFromExperience(experience);
            var pace = GetPaceFromTime(timeAvailable);
            
            return $"A comprehensive {timeline} development plan designed to transform you into a skilled {careerGoal}. " +
                   $"This {pace}-paced roadmap considers your {experience} background and {workStyle.ToLower()} work preference, " +
                   $"focusing on both technical mastery and essential soft skills for career success.";
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

        private string GetLevelFromExperience(string experience)
        {
            return experience switch
            {
                "0-2 years" => "Beginner's",
                "3-5 years" => "Intermediate",
                "5-10 years" => "Advanced",
                "10+ years" => "Expert",
                _ => "Progressive"
            };
        }

        private string GetPaceFromTime(string timeAvailable)
        {
            return timeAvailable switch
            {
                "1-5 hours" => "steady",
                "5-10 hours" => "moderate",
                "10-20 hours" => "intensive",
                "20+ hours" => "accelerated",
                _ => "flexible"
            };
        }

        private List<AiCareerStep> GenerateIntelligentSteps(string careerGoal, string experience, string skillsToLearn)
        {
            var isExperienced = experience.Contains("5-10") || experience.Contains("10+");
            var isBeginner = experience.Contains("0-2");

            return new List<AiCareerStep>
            {
                new AiCareerStep
                {
                    Name = isBeginner ? "Foundation & Setup" : "Advanced Foundation",
                    Description = isBeginner 
                        ? $"Establish strong fundamentals in programming and set up your development environment for {careerGoal}."
                        : $"Refresh core concepts and prepare your skillset for transition to {careerGoal}.",
                    OrderIndex = 0,
                    SkillNames = isBeginner 
                        ? new List<string> { "Git", "Programming Basics", "Development Tools", "Time Management" }
                        : new List<string> { "Git", "Advanced Programming", "Architecture Basics", "Problem Solving" }
                },
                new AiCareerStep
                {
                    Name = "Core Technical Skills",
                    Description = $"Master the essential technical skills and frameworks required for {careerGoal}.",
                    OrderIndex = 1,
                    SkillNames = new List<string> { "C#", "ASP.NET Core", "SQL Database", "REST APIs", "Communication" }
                },
                new AiCareerStep
                {
                    Name = "Advanced Topics & Best Practices",
                    Description = "Dive into advanced concepts, design patterns, and industry-standard practices.",
                    OrderIndex = 2,
                    SkillNames = new List<string> { "Design Patterns", "Microservices", "CI/CD", "Docker", "Leadership" }
                },
                new AiCareerStep
                {
                    Name = "Real-World Projects & Portfolio",
                    Description = "Build production-ready projects, create a strong portfolio, and gain practical experience.",
                    OrderIndex = 3,
                    SkillNames = new List<string> { "System Design", "Cloud Services", "Testing", "Teamwork" }
                }
            };
        }

        private List<AiCareerSkill> GenerateContextualSkills(string careerGoal, string skillsToLearn)
        {
            var skills = new List<AiCareerSkill>
            {
                // Core Technical Skills (always included)
                new AiCareerSkill { Name = "Git", Category = "Technical" },
                new AiCareerSkill { Name = "C#", Category = "Technical" },
                new AiCareerSkill { Name = "ASP.NET Core", Category = "Technical" },
                new AiCareerSkill { Name = "SQL Database", Category = "Technical" },
                new AiCareerSkill { Name = "REST APIs", Category = "Technical" },
                new AiCareerSkill { Name = "Design Patterns", Category = "Technical" },
            };

            // Add contextual technical skills based on user input
            if (skillsToLearn.Contains("Cloud", StringComparison.OrdinalIgnoreCase) || 
                skillsToLearn.Contains("Azure", StringComparison.OrdinalIgnoreCase) ||
                skillsToLearn.Contains("AWS", StringComparison.OrdinalIgnoreCase))
            {
                skills.Add(new AiCareerSkill { Name = "Cloud Services", Category = "Technical" });
            }
            else
            {
                skills.Add(new AiCareerSkill { Name = "Cloud Services", Category = "Technical" });
            }

            if (skillsToLearn.Contains("Docker", StringComparison.OrdinalIgnoreCase) ||
                skillsToLearn.Contains("Container", StringComparison.OrdinalIgnoreCase))
            {
                skills.Add(new AiCareerSkill { Name = "Docker", Category = "Technical" });
            }
            else
            {
                skills.Add(new AiCareerSkill { Name = "Docker", Category = "Technical" });
            }

            if (skillsToLearn.Contains("Microservices", StringComparison.OrdinalIgnoreCase) ||
                careerGoal.Contains("Architect", StringComparison.OrdinalIgnoreCase))
            {
                skills.Add(new AiCareerSkill { Name = "Microservices", Category = "Technical" });
            }
            else
            {
                skills.Add(new AiCareerSkill { Name = "Microservices", Category = "Technical" });
            }

            // Add more technical skills to reach 12
            skills.AddRange(new List<AiCareerSkill>
            {
                new AiCareerSkill { Name = "CI/CD", Category = "Technical" },
                new AiCareerSkill { Name = "System Design", Category = "Technical" },
                new AiCareerSkill { Name = "Testing", Category = "Technical" },
                new AiCareerSkill { Name = careerGoal.Contains("Full Stack") ? "React" : "Development Tools", Category = "Technical" }
            });

            // Add Soft Skills (4 essential)
            skills.AddRange(new List<AiCareerSkill>
            {
                new AiCareerSkill { Name = "Time Management", Category = "Soft" },
                new AiCareerSkill { Name = "Problem Solving", Category = "Soft" },
                new AiCareerSkill { Name = "Communication", Category = "Soft" },
                new AiCareerSkill { Name = careerGoal.Contains("Senior") || careerGoal.Contains("Lead") ? "Leadership" : "Teamwork", Category = "Soft" }
            });

            return skills;
        }

        private class QuizAnswer
        {
            public string QuestionId { get; set; } = string.Empty;
            public string Answer { get; set; } = string.Empty;
        }
    }
}
