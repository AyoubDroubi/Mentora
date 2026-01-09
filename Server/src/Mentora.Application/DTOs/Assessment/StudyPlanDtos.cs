namespace Mentora.Application.DTOs.Assessment
{
    /// <summary>
    /// DTO for AI-generated study plan per SRS 3.2
    /// </summary>
    public class StudyPlanDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? AssessmentAttemptId { get; set; }
        public Guid? CareerPlanId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public DateTime? TargetCompletionDate { get; set; }
        public int EstimatedHours { get; set; }
        public string DifficultyLevel { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int ProgressPercentage { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool IsActive { get; set; }
        public string AiModel { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // Nested data
        public List<StudyPlanStepDto> Steps { get; set; } = new();
        public List<StudyPlanSkillDto> RequiredSkills { get; set; } = new();
        public List<StudyPlanResourceDto> Resources { get; set; } = new();
    }

    /// <summary>
    /// DTO for study plan step per SRS 3.2.2
    /// </summary>
    public class StudyPlanStepDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
        public int EstimatedHours { get; set; }
        public string Status { get; set; } = string.Empty;
        public int ProgressPercentage { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public List<string>? Objectives { get; set; }
        public List<StudyPlanCheckpointDto> Checkpoints { get; set; } = new();
    }

    /// <summary>
    /// DTO for study plan checkpoint per SRS 3.2.2
    /// </summary>
    public class StudyPlanCheckpointDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
        public int EstimatedMinutes { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string Type { get; set; } = string.Empty;
        public bool IsMandatory { get; set; }
        public string? Notes { get; set; }
    }

    /// <summary>
    /// DTO for study plan resource per SRS 3.2.3
    /// </summary>
    public class StudyPlanResourceDto
    {
        public Guid Id { get; set; }
        public Guid? StudyPlanStepId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string ResourceType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? EstimatedHours { get; set; }
        public string? DifficultyLevel { get; set; }
        public bool IsFree { get; set; }
        public decimal? Cost { get; set; }
        public string? Provider { get; set; }
        public int Priority { get; set; }
        public bool IsCompleted { get; set; }
        public int? UserRating { get; set; }
        public List<string>? Tags { get; set; }
    }

    /// <summary>
    /// DTO for study plan skill per SRS 3.3
    /// </summary>
    public class StudyPlanSkillDto
    {
        public Guid SkillId { get; set; }
        public string SkillName { get; set; } = string.Empty;
        public string TargetProficiency { get; set; } = string.Empty;
        public int Importance { get; set; }
        public bool IsPrerequisite { get; set; }
        public string Status { get; set; } = string.Empty;
        public int ProficiencyGap { get; set; }
        public string? CurrentUserLevel { get; set; }
    }

    /// <summary>
    /// Request DTO for generating study plan per SRS 3.2.1
    /// </summary>
    public class GenerateStudyPlanRequestDto
    {
        public Guid AssessmentAttemptId { get; set; }
        public Guid? CareerPlanId { get; set; }
        public DateTime? PreferredCompletionDate { get; set; }
        public int? WeeklyHoursAvailable { get; set; }
        public List<string>? FocusAreas { get; set; }
        public string? AdditionalInstructions { get; set; }
    }

    /// <summary>
    /// Response DTO for study plan generation
    /// </summary>
    public class GenerateStudyPlanResponseDto
    {
        public Guid StudyPlanId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int TotalSteps { get; set; }
        public int TotalResources { get; set; }
        public int EstimatedHours { get; set; }
        public List<string> RequiredSkills { get; set; } = new();
        public DateTime GeneratedAt { get; set; }
    }

    /// <summary>
    /// DTO for updating study plan progress per SRS 5.2
    /// </summary>
    public class UpdateStudyPlanProgressDto
    {
        public Guid StudyPlanId { get; set; }
        public Guid? StepId { get; set; }
        public Guid? CheckpointId { get; set; }
        public string? NewStatus { get; set; }
        public bool? IsCompleted { get; set; }
        public int? ProgressPercentage { get; set; }
    }

    /// <summary>
    /// DTO for skill gap analysis per SRS 3.3.3
    /// </summary>
    public class SkillGapAnalysisDto
    {
        public Guid StudyPlanId { get; set; }
        public int TotalRequiredSkills { get; set; }
        public int MissingSkills { get; set; }
        public int InProgressSkills { get; set; }
        public int AchievedSkills { get; set; }
        public List<SkillGapDto> Gaps { get; set; } = new();
        public List<string> Recommendations { get; set; } = new();
    }

    /// <summary>
    /// Individual skill gap detail
    /// </summary>
    public class SkillGapDto
    {
        public string SkillName { get; set; } = string.Empty;
        public string CurrentLevel { get; set; } = string.Empty;
        public string TargetLevel { get; set; } = string.Empty;
        public int Gap { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public List<string> RecommendedResources { get; set; } = new();
    }
}
