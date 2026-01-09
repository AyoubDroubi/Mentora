using Mentora.Domain.Entities.Assessment;
using System.Text.Json;

namespace Mentora.Infrastructure.Data
{
    /// <summary>
    /// Seed data for Assessment Questions per SRS 3.1.1
    /// Provides initial diagnostic questions for different majors
    /// </summary>
    public static class AssessmentQuestionSeeder
    {
        public static List<AssessmentQuestion> GetInitialQuestions()
        {
            var questions = new List<AssessmentQuestion>();
            int orderIndex = 1;

            // =================================================================================
            // GENERAL QUESTIONS (For all majors)
            // =================================================================================

            questions.Add(new AssessmentQuestion
            {
                QuestionText = "How many years until you graduate?",
                QuestionType = QuestionType.Scale,
                TargetMajor = null, // Applies to all majors
                Category = "Timeline",
                OrderIndex = orderIndex++,
                IsRequired = true,
                MinValue = 0,
                MaxValue = 6,
                HelpText = "This helps us plan your study timeline appropriately.",
                IsActive = true
            });

            questions.Add(new AssessmentQuestion
            {
                QuestionText = "How many hours per week can you dedicate to studying?",
                QuestionType = QuestionType.Scale,
                TargetMajor = null,
                Category = "Availability",
                OrderIndex = orderIndex++,
                IsRequired = true,
                MinValue = 1,
                MaxValue = 40,
                HelpText = "Be realistic about your available time commitment.",
                IsActive = true
            });

            questions.Add(new AssessmentQuestion
            {
                QuestionText = "What is your current GPA or academic standing?",
                QuestionType = QuestionType.MultipleChoice, // Fixed
                TargetMajor = null,
                Category = "Academic Background",
                OrderIndex = orderIndex++,
                IsRequired = true,
                OptionsJson = JsonSerializer.Serialize(new[]
                {
                    "Below 2.0",
                    "2.0 - 2.5",
                    "2.5 - 3.0",
                    "3.0 - 3.5",
                    "3.5 - 4.0"
                }),
                IsActive = true
            });

            questions.Add(new AssessmentQuestion
            {
                QuestionText = "What times of day are you most productive for studying?",
                QuestionType = QuestionType.MultipleChoice,
                TargetMajor = null,
                Category = "Study Preferences",
                OrderIndex = orderIndex++,
                IsRequired = false,
                OptionsJson = JsonSerializer.Serialize(new[]
                {
                    "Early Morning (5-9 AM)",
                    "Late Morning (9-12 PM)",
                    "Afternoon (12-5 PM)",
                    "Evening (5-9 PM)",
                    "Night (9 PM-1 AM)",
                    "Late Night (1-5 AM)"
                }),
                HelpText = "Select all that apply. This helps us schedule tasks effectively.",
                IsActive = true
            });

            // =================================================================================
            // COMPUTER SCIENCE SPECIFIC QUESTIONS
            // =================================================================================

            questions.Add(new AssessmentQuestion
            {
                QuestionText = "What programming languages are you familiar with?",
                QuestionType = QuestionType.MultipleChoice,
                TargetMajor = "Computer Science",
                Category = "Technical Skills",
                OrderIndex = orderIndex++,
                IsRequired = true,
                OptionsJson = JsonSerializer.Serialize(new[]
                {
                    "C#",
                    "Java",
                    "Python",
                    "JavaScript/TypeScript",
                    "C/C++",
                    "PHP",
                    "Ruby",
                    "Go",
                    "Rust",
                    "None"
                }),
                HelpText = "Select all languages you have practical experience with.",
                IsActive = true
            });

            questions.Add(new AssessmentQuestion
            {
                QuestionText = "Which areas of computer science interest you most?",
                QuestionType = QuestionType.MultipleChoice,
                TargetMajor = "Computer Science",
                Category = "Interests",
                OrderIndex = orderIndex++,
                IsRequired = true,
                OptionsJson = JsonSerializer.Serialize(new[]
                {
                    "Web Development",
                    "Mobile Development",
                    "Data Science & AI",
                    "Cybersecurity",
                    "Cloud Computing",
                    "Game Development",
                    "DevOps",
                    "Blockchain",
                    "IoT",
                    "Systems Programming"
                }),
                HelpText = "Choose up to 3 areas that interest you most.",
                IsActive = true
            });

            questions.Add(new AssessmentQuestion
            {
                QuestionText = "Have you worked on any coding projects before?",
                QuestionType = QuestionType.MultipleChoice, // Fixed
                TargetMajor = "Computer Science",
                Category = "Experience",
                OrderIndex = orderIndex++,
                IsRequired = true,
                OptionsJson = JsonSerializer.Serialize(new[]
                {
                    "No, I'm a complete beginner",
                    "Yes, small personal projects",
                    "Yes, school/university projects",
                    "Yes, internship/work projects",
                    "Yes, multiple substantial projects"
                }),
                IsActive = true
            });

            questions.Add(new AssessmentQuestion
            {
                QuestionText = "What is your biggest challenge in learning programming?",
                QuestionType = QuestionType.MultipleChoice, // Fixed
                TargetMajor = "Computer Science",
                Category = "Challenges",
                OrderIndex = orderIndex++,
                IsRequired = false,
                OptionsJson = JsonSerializer.Serialize(new[]
                {
                    "Understanding core concepts",
                    "Debugging and problem-solving",
                    "Choosing what to learn",
                    "Staying motivated",
                    "Finding time to practice",
                    "Understanding documentation",
                    "Building real projects"
                }),
                IsActive = true
            });

            // =================================================================================
            // BUSINESS ADMINISTRATION SPECIFIC QUESTIONS
            // =================================================================================

            questions.Add(new AssessmentQuestion
            {
                QuestionText = "Which business area interests you most?",
                QuestionType = QuestionType.MultipleChoice,
                TargetMajor = "Business Administration",
                Category = "Interests",
                OrderIndex = orderIndex++,
                IsRequired = true,
                OptionsJson = JsonSerializer.Serialize(new[]
                {
                    "Marketing",
                    "Finance",
                    "Human Resources",
                    "Operations Management",
                    "Entrepreneurship",
                    "Project Management",
                    "Business Analytics",
                    "International Business"
                }),
                HelpText = "Select your top 3 areas of interest.",
                IsActive = true
            });

            questions.Add(new AssessmentQuestion
            {
                QuestionText = "Do you have any prior business or work experience?",
                QuestionType = QuestionType.MultipleChoice, // Fixed
                TargetMajor = "Business Administration",
                Category = "Experience",
                OrderIndex = orderIndex++,
                IsRequired = true,
                OptionsJson = JsonSerializer.Serialize(new[]
                {
                    "No experience",
                    "Part-time work (unrelated)",
                    "Internship in business field",
                    "Full-time work experience",
                    "Entrepreneur/Startup experience"
                }),
                IsActive = true
            });

            // =================================================================================
            // ENGINEERING SPECIFIC QUESTIONS
            // =================================================================================

            questions.Add(new AssessmentQuestion
            {
                QuestionText = "Which engineering discipline are you studying?",
                QuestionType = QuestionType.MultipleChoice, // Fixed
                TargetMajor = "Engineering",
                Category = "Specialization",
                OrderIndex = orderIndex++,
                IsRequired = true,
                OptionsJson = JsonSerializer.Serialize(new[]
                {
                    "Mechanical Engineering",
                    "Electrical Engineering",
                    "Civil Engineering",
                    "Chemical Engineering",
                    "Software Engineering",
                    "Aerospace Engineering",
                    "Biomedical Engineering",
                    "Industrial Engineering",
                    "Other"
                }),
                IsActive = true
            });

            questions.Add(new AssessmentQuestion
            {
                QuestionText = "Which tools or software are you familiar with?",
                QuestionType = QuestionType.MultipleChoice,
                TargetMajor = "Engineering",
                Category = "Technical Skills",
                OrderIndex = orderIndex++,
                IsRequired = false,
                OptionsJson = JsonSerializer.Serialize(new[]
                {
                    "AutoCAD",
                    "MATLAB",
                    "SolidWorks",
                    "LabVIEW",
                    "ANSYS",
                    "SPICE",
                    "Python for Engineering",
                    "Excel/Data Analysis",
                    "None yet"
                }),
                IsActive = true
            });

            // =================================================================================
            // MEDICINE SPECIFIC QUESTIONS
            // =================================================================================

            questions.Add(new AssessmentQuestion
            {
                QuestionText = "Which medical specialty interests you most?",
                QuestionType = QuestionType.MultipleChoice,
                TargetMajor = "Medicine",
                Category = "Interests",
                OrderIndex = orderIndex++,
                IsRequired = false,
                OptionsJson = JsonSerializer.Serialize(new[]
                {
                    "Internal Medicine",
                    "Surgery",
                    "Pediatrics",
                    "Cardiology",
                    "Neurology",
                    "Psychiatry",
                    "Radiology",
                    "Emergency Medicine",
                    "General Practice",
                    "Not sure yet"
                }),
                HelpText = "It's okay if you're not sure yet!",
                IsActive = true
            });

            questions.Add(new AssessmentQuestion
            {
                QuestionText = "What study resources do you primarily use?",
                QuestionType = QuestionType.MultipleChoice,
                TargetMajor = "Medicine",
                Category = "Study Methods",
                OrderIndex = orderIndex++,
                IsRequired = false,
                OptionsJson = JsonSerializer.Serialize(new[]
                {
                    "Textbooks",
                    "Online Videos (YouTube, etc.)",
                    "Question Banks (UWorld, etc.)",
                    "Flashcards (Anki, etc.)",
                    "Study Groups",
                    "Medical Apps",
                    "Research Papers"
                }),
                IsActive = true
            });

            // =================================================================================
            // FINAL OPEN-ENDED QUESTION (For all majors)
            // =================================================================================

            questions.Add(new AssessmentQuestion
            {
                QuestionText = "What are your specific academic goals for this semester?",
                QuestionType = QuestionType.Text, // Fixed from OpenEnded
                TargetMajor = null,
                Category = "Goals",
                OrderIndex = orderIndex++,
                IsRequired = false,
                HelpText = "Describe what you want to achieve academically. Be specific!",
                ValidationPattern = "^.{10,500}$", // Min 10 chars, max 500 chars
                IsActive = true
            });

            questions.Add(new AssessmentQuestion
            {
                QuestionText = "Is there anything else we should know to create your personalized study plan?",
                QuestionType = QuestionType.Text, // Fixed from OpenEnded
                TargetMajor = null,
                Category = "Additional Info",
                OrderIndex = orderIndex++,
                IsRequired = false,
                HelpText = "Any learning disabilities, preferences, or special circumstances.",
                IsActive = true
            });

            return questions;
        }
    }
}
