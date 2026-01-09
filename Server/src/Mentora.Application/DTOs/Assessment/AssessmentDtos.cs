namespace Mentora.Application.DTOs.Assessment
{
    /// <summary>
    /// DTO for retrieving assessment questions per SRS 3.1.1
    /// </summary>
    public class AssessmentQuestionDto
    {
        public Guid Id { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string QuestionType { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
        public bool IsRequired { get; set; }
        public List<string>? Options { get; set; }
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
        public string? HelpText { get; set; }
        public string? ValidationPattern { get; set; }
    }

    /// <summary>
    /// DTO for submitting assessment responses per SRS 3.1.2
    /// </summary>
    public class SubmitAssessmentResponseDto
    {
        public Guid QuestionId { get; set; }
        public string ResponseValue { get; set; } = string.Empty;
        public bool IsSkipped { get; set; } = false;
        public string? Notes { get; set; }
    }

    /// <summary>
    /// DTO for creating or updating assessment attempt
    /// </summary>
    public class AssessmentAttemptDto
    {
        public Guid? Id { get; set; }
        public string Major { get; set; } = string.Empty;
        public string StudyLevel { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int CompletionPercentage { get; set; }
    }

    /// <summary>
    /// DTO for bulk assessment submission per SRS 3.1.2
    /// </summary>
    public class BulkAssessmentSubmissionDto
    {
        public Guid AssessmentAttemptId { get; set; }
        public List<SubmitAssessmentResponseDto> Responses { get; set; } = new();
    }

    /// <summary>
    /// DTO for assessment context serialization per SRS 3.1.2
    /// Simplified format for AI prompt injection
    /// </summary>
    public class AssessmentContextDto
    {
        public string Major { get; set; } = string.Empty;
        public string StudyLevel { get; set; } = string.Empty;
        public int YearsUntilGraduation { get; set; }
        public List<string> CurrentSkills { get; set; } = new();
        public List<string> InterestedAreas { get; set; } = new();
        public string CareerGoal { get; set; } = string.Empty;
        public int WeeklyHoursAvailable { get; set; }
        public string LearningStyle { get; set; } = string.Empty;
        public Dictionary<string, string> AdditionalContext { get; set; } = new();
    }

    /// <summary>
    /// Response DTO for assessment completion
    /// </summary>
    public class AssessmentCompletionResponseDto
    {
        public Guid AssessmentAttemptId { get; set; }
        public string Status { get; set; } = string.Empty;
        public int TotalQuestions { get; set; }
        public int AnsweredQuestions { get; set; }
        public int CompletionPercentage { get; set; }
        public string? ContextJson { get; set; }
        public bool CanGenerateStudyPlan { get; set; }
    }
}
