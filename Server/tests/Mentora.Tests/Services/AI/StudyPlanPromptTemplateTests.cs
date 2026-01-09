using Mentora.Application.DTOs.Assessment;
using Mentora.Application.Services.AI;
using System.Text.Json;
using Xunit;

namespace Mentora.Tests.Services.AI
{
    /// <summary>
    /// Unit tests for StudyPlanPromptTemplate per SRS 3.2.1
    /// Tests prompt generation and response validation
    /// </summary>
    public class StudyPlanPromptTemplateTests
    {
        private readonly StudyPlanPromptTemplate _promptTemplate;

        public StudyPlanPromptTemplateTests()
        {
            _promptTemplate = new StudyPlanPromptTemplate();
        }

        [Fact]
        public void GetSystemPrompt_ShouldContainRequiredElements()
        {
            // Act
            var systemPrompt = _promptTemplate.GetSystemPrompt();

            // Assert
            Assert.Contains("JSON Schema", systemPrompt);
            Assert.Contains("title", systemPrompt);
            Assert.Contains("summary", systemPrompt);
            Assert.Contains("steps", systemPrompt);
            Assert.Contains("requiredSkills", systemPrompt);
            Assert.Contains("resources", systemPrompt);
            Assert.Contains("STRICT OUTPUT REQUIREMENTS", systemPrompt);
        }

        [Fact]
        public void BuildPrompt_WithFullContext_ShouldIncludeAllSections()
        {
            // Arrange
            var context = new AssessmentContextDto
            {
                Major = "Computer Science",
                StudyLevel = "Junior",
                YearsUntilGraduation = 2,
                CurrentSkills = new List<string> { "C#", "SQL" },
                InterestedAreas = new List<string> { "Web Development" },
                CareerGoal = "Full Stack Developer",
                WeeklyHoursAvailable = 15,
                LearningStyle = "Hands-on"
            };

            // Act
            var prompt = _promptTemplate.BuildPrompt(context);

            // Assert
            Assert.Contains("STUDENT CONTEXT:", prompt);
            Assert.Contains("Computer Science", prompt);
            Assert.Contains("CONSTRAINTS:", prompt);
            Assert.Contains("2 years", prompt);
            Assert.Contains("15 hours", prompt);
            Assert.Contains("LEARNING GOALS:", prompt);
            Assert.Contains("Full Stack Developer", prompt);
        }

        [Fact]
        public void BuildPrompt_WithAdditionalInstructions_ShouldIncludeThem()
        {
            // Arrange
            var context = new AssessmentContextDto
            {
                Major = "Computer Science",
                StudyLevel = "Junior"
            };
            var additionalInstructions = "Focus on frontend technologies and modern frameworks";

            // Act
            var prompt = _promptTemplate.BuildPrompt(context, additionalInstructions);

            // Assert
            Assert.Contains("ADDITIONAL INSTRUCTIONS:", prompt);
            Assert.Contains("Focus on frontend technologies", prompt);
        }

        [Fact]
        public void BuildPrompt_WithoutAdditionalInstructions_ShouldNotIncludeSection()
        {
            // Arrange
            var context = new AssessmentContextDto
            {
                Major = "Computer Science",
                StudyLevel = "Junior"
            };

            // Act
            var prompt = _promptTemplate.BuildPrompt(context, null);

            // Assert
            Assert.DoesNotContain("ADDITIONAL INSTRUCTIONS:", prompt);
        }

        [Fact]
        public void BuildPrompt_ShouldCalculateTotalAvailableHours()
        {
            // Arrange
            var context = new AssessmentContextDto
            {
                Major = "Computer Science",
                StudyLevel = "Junior",
                YearsUntilGraduation = 1,
                WeeklyHoursAvailable = 10
            };

            // Act
            var prompt = _promptTemplate.BuildPrompt(context);

            // Assert - Should show total hours calculation (1 year * 48 weeks * 10 hours = 480)
            Assert.Contains("Total available study hours:", prompt);
            Assert.Contains("480", prompt);
        }

        [Fact]
        public void ValidateResponse_WithValidJson_ShouldReturnTrue()
        {
            // Arrange
            var validJson = @"{
                ""title"": ""Full Stack Developer Roadmap"",
                ""summary"": ""A comprehensive study plan for full stack development"",
                ""estimatedHours"": 300,
                ""difficultyLevel"": ""Intermediate"",
                ""steps"": [
                    {
                        ""name"": ""Frontend Basics"",
                        ""description"": ""Learn HTML, CSS, JavaScript"",
                        ""orderIndex"": 0,
                        ""estimatedHours"": 80,
                        ""objectives"": [""Master HTML5"", ""Learn CSS3""],
                        ""checkpoints"": [
                            {
                                ""description"": ""Complete HTML tutorial"",
                                ""orderIndex"": 0,
                                ""estimatedMinutes"": 120,
                                ""type"": ""Tutorial"",
                                ""isMandatory"": true
                            }
                        ]
                    }
                ],
                ""requiredSkills"": [
                    {
                        ""skillName"": ""HTML"",
                        ""targetProficiency"": ""Intermediate"",
                        ""importance"": 4,
                        ""isPrerequisite"": true
                    }
                ],
                ""resources"": [
                    {
                        ""title"": ""MDN Web Docs"",
                        ""url"": ""https://developer.mozilla.org"",
                        ""resourceType"": ""Documentation"",
                        ""description"": ""Official web documentation"",
                        ""estimatedHours"": 20,
                        ""difficultyLevel"": ""Beginner"",
                        ""isFree"": true,
                        ""provider"": ""Mozilla"",
                        ""priority"": 5
                    }
                ]
            }";

            // Act
            var result = _promptTemplate.ValidateResponse(validJson, out var error);

            // Assert
            Assert.True(result);
            Assert.Null(error);
        }

        [Fact]
        public void ValidateResponse_WithMissingTitle_ShouldReturnFalse()
        {
            // Arrange
            var invalidJson = @"{
                ""summary"": ""A study plan"",
                ""steps"": []
            }";

            // Act
            var result = _promptTemplate.ValidateResponse(invalidJson, out var error);

            // Assert
            Assert.False(result);
            Assert.NotNull(error);
            Assert.Contains("title", error);
        }

        [Fact]
        public void ValidateResponse_WithEmptySteps_ShouldReturnFalse()
        {
            // Arrange
            var invalidJson = @"{
                ""title"": ""Study Plan"",
                ""summary"": ""A study plan"",
                ""steps"": []
            }";

            // Act
            var result = _promptTemplate.ValidateResponse(invalidJson, out var error);

            // Assert
            Assert.False(result);
            Assert.NotNull(error);
            Assert.Contains("steps", error);
        }

        [Fact]
        public void ValidateResponse_WithNoCheckpoints_ShouldReturnFalse()
        {
            // Arrange
            var invalidJson = @"{
                ""title"": ""Study Plan"",
                ""summary"": ""A study plan"",
                ""steps"": [
                    {
                        ""name"": ""Step 1"",
                        ""description"": ""First step"",
                        ""checkpoints"": []
                    }
                ]
            }";

            // Act
            var result = _promptTemplate.ValidateResponse(invalidJson, out var error);

            // Assert
            Assert.False(result);
            Assert.NotNull(error);
            Assert.Contains("checkpoints", error);
        }

        [Fact]
        public void ValidateResponse_WithInvalidJson_ShouldReturnFalse()
        {
            // Arrange
            var invalidJson = "{ this is not valid json }";

            // Act
            var result = _promptTemplate.ValidateResponse(invalidJson, out var error);

            // Assert
            Assert.False(result);
            Assert.NotNull(error);
            Assert.Contains("Invalid JSON", error);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(4)]
        public void BuildPrompt_WithDifferentTimeframes_ShouldAdjustGoals(int yearsUntilGraduation)
        {
            // Arrange
            var context = new AssessmentContextDto
            {
                Major = "Computer Science",
                StudyLevel = "Junior",
                YearsUntilGraduation = yearsUntilGraduation,
                WeeklyHoursAvailable = 10
            };

            // Act
            var prompt = _promptTemplate.BuildPrompt(context);

            // Assert
            Assert.Contains("LEARNING GOALS:", prompt);
            
            if (yearsUntilGraduation <= 1)
                Assert.Contains("job market entry", prompt.ToLower());
            else if (yearsUntilGraduation <= 2)
                Assert.Contains("internship", prompt.ToLower());
        }

        [Fact]
        public void BuildPrompt_WithNoCareerGoal_ShouldUseDefaultMessage()
        {
            // Arrange
            var context = new AssessmentContextDto
            {
                Major = "Computer Science",
                StudyLevel = "Junior",
                CareerGoal = null
            };

            // Act
            var prompt = _promptTemplate.BuildPrompt(context);

            // Assert
            Assert.Contains("Career Goal: Not specified", prompt);
        }

        [Fact]
        public void BuildPrompt_WithNoCurrentSkills_ShouldIndicateNone()
        {
            // Arrange
            var context = new AssessmentContextDto
            {
                Major = "Computer Science",
                StudyLevel = "Junior",
                CurrentSkills = new List<string>()
            };

            // Act
            var prompt = _promptTemplate.BuildPrompt(context);

            // Assert
            Assert.Contains("Current Skills: None specified", prompt);
        }
    }
}
