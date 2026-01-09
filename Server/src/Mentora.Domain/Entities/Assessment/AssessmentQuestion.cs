using Mentora.Domain.Common;

namespace Mentora.Domain.Entities.Assessment
{
    /// <summary>
    /// Dynamic diagnostic questions based on user's major per SRS 3.1.1
    /// Supports flexible question types for comprehensive assessment
    /// </summary>
    public class AssessmentQuestion : BaseEntity
    {
        /// <summary>
        /// Question text to display to user
        /// </summary>
        public string QuestionText { get; set; } = string.Empty;

        /// <summary>
        /// Type of question (MultipleChoice, Scale, Text, DateRange)
        /// </summary>
        public QuestionType QuestionType { get; set; }

        /// <summary>
        /// Target major for this question (null = applies to all majors)
        /// </summary>
        public string? TargetMajor { get; set; }

        /// <summary>
        /// Category for grouping (e.g., "Skills", "Experience", "Goals", "Constraints")
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Display order in the assessment flow
        /// </summary>
        public int OrderIndex { get; set; }

        /// <summary>
        /// Whether this question is required
        /// </summary>
        public bool IsRequired { get; set; } = true;

        /// <summary>
        /// JSON string containing question options for MultipleChoice
        /// Format: ["Option 1", "Option 2", "Option 3"]
        /// </summary>
        public string? OptionsJson { get; set; }

        /// <summary>
        /// Minimum value for Scale type questions
        /// </summary>
        public int? MinValue { get; set; }

        /// <summary>
        /// Maximum value for Scale type questions
        /// </summary>
        public int? MaxValue { get; set; }

        /// <summary>
        /// Helper text or description for the question
        /// </summary>
        public string? HelpText { get; set; }

        /// <summary>
        /// JSON string for conditional display logic
        /// Format: {"dependsOn": "questionId", "expectedValue": "value"}
        /// </summary>
        public string? ConditionalLogicJson { get; set; }

        /// <summary>
        /// Validation regex pattern for Text type questions
        /// </summary>
        public string? ValidationPattern { get; set; }

        /// <summary>
        /// Whether this question is active and should be displayed
        /// </summary>
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ICollection<AssessmentResponse> Responses { get; set; } = new List<AssessmentResponse>();
    }

    /// <summary>
    /// Question type enum per SRS design principles
    /// </summary>
    public enum QuestionType
    {
        /// <summary>
        /// Single or multiple choice selection
        /// </summary>
        MultipleChoice = 0,

        /// <summary>
        /// Numeric scale (e.g., 1-10)
        /// </summary>
        Scale = 1,

        /// <summary>
        /// Free text input
        /// </summary>
        Text = 2,

        /// <summary>
        /// Date or date range selection
        /// </summary>
        DateRange = 3,

        /// <summary>
        /// Yes/No boolean question
        /// </summary>
        Boolean = 4
    }
}
