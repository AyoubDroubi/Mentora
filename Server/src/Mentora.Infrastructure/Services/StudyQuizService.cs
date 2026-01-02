using Mentora.Application.DTOs.StudyPlanner;
using Mentora.Application.Interfaces.Repositories;
using Mentora.Application.Interfaces.Services;
using Mentora.Domain.Entities.StudyPlanner;
using System.Text.Json;

namespace Mentora.Infrastructure.Services
{
    /// <summary>
    /// Service implementation for Study Quiz operations
    /// Per SRS Study Planner - Feature 6: Study Quiz (Diagnostic)
    /// </summary>
    public class StudyQuizService : IStudyQuizService
    {
        private readonly IStudyQuizRepository _quizRepository;

        public StudyQuizService(IStudyQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public Task<IEnumerable<object>> GetQuestionsAsync()
        {
            var questions = new List<object>
            {
                new { id = 1, question = "What are your top 3 academic or learning goals for the next 30–90 days?", type = "open-ended" },
                new { id = 2, question = "Which subject or skill do you want to improve the most, and why?", type = "open-ended" },
                new { id = 3, question = "How many hours per day can you realistically study?", type = "single-choice", options = new[] { "Less than 1 hour", "1–2 hours", "2–3 hours", "3–4 hours", "More than 4 hours" } },
                new { id = 4, question = "At what times of day do you feel most productive?", type = "multiple-choice", options = new[] { "Early morning (5–9 AM)", "Late morning (9–12 PM)", "Afternoon (12–5 PM)", "Evening (5–9 PM)", "Night (9 PM–1 AM)" } },
                new { id = 5, question = "Which days of the week are available for studying?", type = "multiple-choice", options = new[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" } },
                new { id = 6, question = "How do you usually plan your study sessions?", type = "single-choice", options = new[] { "I don't plan; I study when I feel like it", "Rough plan in my head", "To-do list or notes app", "Calendar or time-blocking", "Strict daily schedule" } },
                new { id = 7, question = "How do you usually start a study session?", type = "single-choice", options = new[] { "Start immediately", "Delay a little, then start", "Get distracted (phone/social media)", "Wait until I 'feel motivated'", "Avoid starting unless there's pressure" } },
                new { id = 8, question = "How long can you stay focused before needing a break?", type = "single-choice", options = new[] { "Less than 15 minutes", "15–30 minutes", "30–45 minutes", "45–60 minutes", "More than 60 minutes" } },
                new { id = 9, question = "What are the main things that distract you while studying?", type = "multiple-choice", options = new[] { "Phone / social media", "Noise or environment", "Boredom", "Stress or anxiety", "Fatigue", "Overthinking / perfectionism" } },
                new { id = 10, question = "Which subjects or topics do you struggle with the most, and why?", type = "open-ended" },
                new { id = 11, question = "What do you struggle with the most?", type = "single-choice", options = new[] { "Remembering information", "Understanding concepts", "Applying what I learn", "All of the above" } },
                new { id = 12, question = "What is your biggest challenge in studying consistently?", type = "single-choice", options = new[] { "Procrastination", "Lack of motivation", "Poor time management", "Burnout / low energy", "Stress or anxiety", "Distractions" } },
                new { id = 13, question = "Do you have upcoming exams, projects, or deadlines?", type = "open-ended" },
                new { id = 14, question = "How would you describe your study personality?", type = "single-choice", options = new[] { "Very structured and disciplined", "Semi-organized", "Chaotic but motivated", "Procrastinator", "Only motivated close to exams" } }
            };

            return Task.FromResult<IEnumerable<object>>(questions);
        }

        public async Task<StudyQuizResultDto> SubmitQuizAsync(Guid userId, Dictionary<string, string> answers)
        {
            if (answers == null || answers.Count == 0)
                throw new ArgumentException("Answers are required", nameof(answers));

            // Generate personalized study plan
            var studyPlan = GenerateStudyPlan(answers);

            // Save quiz attempt
            var quizAttempt = new StudyQuizAttempt
            {
                UserId = userId,
                AnswersJson = JsonSerializer.Serialize(answers),
                GeneratedPlan = JsonSerializer.Serialize(studyPlan)
            };

            var savedAttempt = await _quizRepository.CreateAsync(quizAttempt);

            return new StudyQuizResultDto
            {
                AttemptId = savedAttempt.Id,
                CreatedAt = savedAttempt.CreatedAt,
                StudyPlan = studyPlan
            };
        }

        public async Task<StudyQuizResultDto?> GetLatestAttemptAsync(Guid userId)
        {
            var attempt = await _quizRepository.GetLatestAttemptAsync(userId);

            if (attempt == null)
                return null;

            var studyPlan = JsonSerializer.Deserialize<object>(attempt.GeneratedPlan);

            return new StudyQuizResultDto
            {
                AttemptId = attempt.Id,
                CreatedAt = attempt.CreatedAt,
                StudyPlan = studyPlan ?? new()
            };
        }

        private object GenerateStudyPlan(Dictionary<string, string> answers)
        {
            var recommendations = new List<string>
            {
                "Set specific, achievable daily goals",
                "Use active recall and spaced repetition",
                "Take regular breaks to maintain focus (Pomodoro technique)",
                "Track your progress weekly"
            };

            var schedule = new List<string>
            {
                "Morning: Review previous day's material (30 min)",
                "Midday: Focus on challenging subjects (90 min)",
                "Evening: Practice and consolidation (60 min)"
            };

            var tips = new List<string>
            {
                "Stay consistent with your study schedule",
                "Get adequate sleep for better concentration",
                "Stay hydrated and maintain healthy eating habits",
                "Review material regularly to reinforce learning"
            };

            // Customize based on answers
            if (answers.ContainsKey("3") && answers["3"].Contains("Less than 1"))
            {
                recommendations.Add("Focus on quality over quantity - make every minute count");
            }

            if (answers.ContainsKey("12") && answers["12"].Contains("Procrastination"))
            {
                tips.Add("Start with the hardest task first to overcome procrastination");
                tips.Add("Use the 2-minute rule: if it takes less than 2 minutes, do it now");
            }

            return new
            {
                title = "Your Personalized Study Plan",
                summary = "Based on your responses, here's a tailored study plan to help you succeed",
                recommendations,
                schedule,
                tips
            };
        }
    }
}
