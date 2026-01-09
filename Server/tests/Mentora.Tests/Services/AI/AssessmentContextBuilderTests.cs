using Mentora.Application.DTOs.Assessment;
using Mentora.Application.Services.AI;
using Mentora.Domain.Entities;
using Mentora.Domain.Entities.Assessment;
using Xunit;

namespace Mentora.Tests.Services.AI
{
    /// <summary>
    /// Unit tests for AssessmentContextBuilder per SRS 3.1.2
    /// Tests context serialization and prompt building
    /// </summary>
    public class AssessmentContextBuilderTests
    {
        private readonly AssessmentContextBuilder _contextBuilder;

        public AssessmentContextBuilderTests()
        {
            _contextBuilder = new AssessmentContextBuilder();
        }

        [Fact]
        public void BuildContext_WithValidResponses_ShouldExtractAllFields()
        {
            // Arrange
            var attempt = new AssessmentAttempt
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Major = "Computer Science",
                StudyLevel = StudyLevel.Junior
            };

            var questions = new List<AssessmentQuestion>
            {
                new() { Id = Guid.NewGuid(), Category = "Timeline", QuestionText = "Years until graduation?" },
                new() { Id = Guid.NewGuid(), Category = "Skills", QuestionText = "Current skills?" },
                new() { Id = Guid.NewGuid(), Category = "Career Goal", QuestionText = "Career goal?" },
                new() { Id = Guid.NewGuid(), Category = "Availability", QuestionText = "Weekly hours?" }
            };

            var responses = new List<AssessmentResponse>
            {
                new()
                {
                    QuestionId = questions[0].Id,
                    Question = questions[0],
                    ResponseValue = "2",
                    IsSkipped = false
                },
                new()
                {
                    QuestionId = questions[1].Id,
                    Question = questions[1],
                    ResponseValue = "C#, SQL, JavaScript",
                    IsSkipped = false
                },
                new()
                {
                    QuestionId = questions[2].Id,
                    Question = questions[2],
                    ResponseValue = "Software Engineer",
                    IsSkipped = false
                },
                new()
                {
                    QuestionId = questions[3].Id,
                    Question = questions[3],
                    ResponseValue = "15",
                    IsSkipped = false
                }
            };

            // Act
            var context = _contextBuilder.BuildContext(attempt, responses);

            // Assert
            Assert.Equal("Computer Science", context.Major);
            Assert.Equal("Junior", context.StudyLevel);
            Assert.Equal(2, context.YearsUntilGraduation);
            Assert.Contains("C#", context.CurrentSkills);
            Assert.Contains("SQL", context.CurrentSkills);
            Assert.Contains("JavaScript", context.CurrentSkills);
            Assert.Equal("Software Engineer", context.CareerGoal);
            Assert.Equal(15, context.WeeklyHoursAvailable);
        }

        [Fact]
        public void BuildContext_WithSkippedResponses_ShouldIgnoreSkipped()
        {
            // Arrange
            var attempt = new AssessmentAttempt
            {
                Major = "Computer Science",
                StudyLevel = StudyLevel.Freshman
            };

            var question = new AssessmentQuestion
            {
                Id = Guid.NewGuid(),
                Category = "Skills",
                QuestionText = "Skills?"
            };

            var responses = new List<AssessmentResponse>
            {
                new()
                {
                    QuestionId = question.Id,
                    Question = question,
                    ResponseValue = "Python",
                    IsSkipped = false
                },
                new()
                {
                    QuestionId = Guid.NewGuid(),
                    Question = question,
                    ResponseValue = "Java",
                    IsSkipped = true // Should be ignored
                }
            };

            // Act
            var context = _contextBuilder.BuildContext(attempt, responses);

            // Assert
            Assert.Single(context.CurrentSkills);
            Assert.Contains("Python", context.CurrentSkills);
            Assert.DoesNotContain("Java", context.CurrentSkills);
        }

        [Fact]
        public void SerializeContext_ShouldProduceValidJson()
        {
            // Arrange
            var context = new AssessmentContextDto
            {
                Major = "Computer Science",
                StudyLevel = "Junior",
                YearsUntilGraduation = 2,
                CurrentSkills = new List<string> { "C#", "SQL" },
                CareerGoal = "Software Engineer",
                WeeklyHoursAvailable = 15
            };

            // Act
            var json = _contextBuilder.SerializeContext(context);

            // Assert
            Assert.NotNull(json);
            Assert.Contains("\"major\"", json);
            Assert.Contains("Computer Science", json);
            Assert.Contains("\"currentSkills\"", json);
        }

        [Fact]
        public void DeserializeContext_ShouldRestoreOriginalContext()
        {
            // Arrange
            var originalContext = new AssessmentContextDto
            {
                Major = "Computer Science",
                StudyLevel = "Junior",
                YearsUntilGraduation = 2,
                CurrentSkills = new List<string> { "C#", "SQL" },
                CareerGoal = "Software Engineer",
                WeeklyHoursAvailable = 15
            };

            var json = _contextBuilder.SerializeContext(originalContext);

            // Act
            var deserializedContext = _contextBuilder.DeserializeContext(json);

            // Assert
            Assert.NotNull(deserializedContext);
            Assert.Equal(originalContext.Major, deserializedContext.Major);
            Assert.Equal(originalContext.StudyLevel, deserializedContext.StudyLevel);
            Assert.Equal(originalContext.YearsUntilGraduation, deserializedContext.YearsUntilGraduation);
            Assert.Equal(originalContext.CurrentSkills.Count, deserializedContext.CurrentSkills.Count);
            Assert.Equal(originalContext.CareerGoal, deserializedContext.CareerGoal);
            Assert.Equal(originalContext.WeeklyHoursAvailable, deserializedContext.WeeklyHoursAvailable);
        }

        [Fact]
        public void BuildPromptContext_ShouldFormatReadablePrompt()
        {
            // Arrange
            var context = new AssessmentContextDto
            {
                Major = "Computer Science",
                StudyLevel = "Junior",
                YearsUntilGraduation = 2,
                CurrentSkills = new List<string> { "C#", "SQL", "JavaScript" },
                InterestedAreas = new List<string> { "Web Development", "AI" },
                CareerGoal = "Full Stack Developer",
                WeeklyHoursAvailable = 15,
                LearningStyle = "Hands-on projects"
            };

            // Act
            var prompt = _contextBuilder.BuildPromptContext(context);

            // Assert
            Assert.Contains("Student Profile:", prompt);
            Assert.Contains("Major: Computer Science", prompt);
            Assert.Contains("Study Level: Junior", prompt);
            Assert.Contains("Time Until Graduation: 2 years", prompt);
            Assert.Contains("Current Skills: C#, SQL, JavaScript", prompt);
            Assert.Contains("Areas of Interest: Web Development, AI", prompt);
            Assert.Contains("Career Goal: Full Stack Developer", prompt);
            Assert.Contains("Weekly Study Time Available: 15 hours", prompt);
            Assert.Contains("Learning Style: Hands-on projects", prompt);
        }

        [Fact]
        public void BuildPromptContext_WithEmptyFields_ShouldHandleGracefully()
        {
            // Arrange
            var context = new AssessmentContextDto
            {
                Major = "Computer Science",
                StudyLevel = "Freshman"
            };

            // Act
            var prompt = _contextBuilder.BuildPromptContext(context);

            // Assert
            Assert.Contains("Student Profile:", prompt);
            Assert.Contains("Major: Computer Science", prompt);
            Assert.DoesNotContain("Current Skills:", prompt); // Should not include empty sections
        }

        [Theory]
        [InlineData("Python,Java,C#", 3)]
        [InlineData("Python\nJava\nC#", 3)]
        [InlineData("[\"Python\",\"Java\",\"C#\"]", 3)]
        [InlineData("Python;Java;C#", 3)]
        public void ParseListResponse_ShouldHandleVariousFormats(string input, int expectedCount)
        {
            // Arrange
            var attempt = new AssessmentAttempt { Major = "CS", StudyLevel = StudyLevel.Junior };
            var question = new AssessmentQuestion
            {
                Id = Guid.NewGuid(),
                Category = "Skills",
                QuestionText = "Skills?"
            };
            var response = new AssessmentResponse
            {
                Question = question,
                ResponseValue = input,
                IsSkipped = false
            };

            // Act
            var context = _contextBuilder.BuildContext(attempt, new List<AssessmentResponse> { response });

            // Assert
            Assert.Equal(expectedCount, context.CurrentSkills.Count);
        }
    }
}
