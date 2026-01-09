using Mentora.Application.DTOs.Assessment;

namespace Mentora.Application.Interfaces.Services
{
    /// <summary>
    /// AI Service interface for generating study plans per SRS Section 3 & 4
    /// </summary>
    public interface IAiStudyPlanService
    {
        /// <summary>
        /// Generate study plan using AI based on assessment context per SRS 3.2
        /// Returns structured study plan data with steps, skills, and resources
        /// </summary>
        Task<AiStudyPlanResponse> GenerateStudyPlanAsync(
            AssessmentContextDto context, 
            string? additionalInstructions = null);

        /// <summary>
        /// Validate AI response structure per SRS 3.2.2
        /// </summary>
        bool ValidateResponse(string aiResponse, out string? error);
    }

    /// <summary>
    /// AI response structure for study plan generation per SRS 3.2
    /// Maps to database entities: AiStudyPlan, StudyPlanStep, etc.
    /// </summary>
    public class AiStudyPlanResponse
    {
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public int EstimatedHours { get; set; }
        public string DifficultyLevel { get; set; } = string.Empty;
        public List<AiStudyPlanStep> Steps { get; set; } = new();
        public List<AiStudyPlanSkill> RequiredSkills { get; set; } = new();
        public List<AiStudyPlanResource> Resources { get; set; } = new();

        // Metadata
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public string AiModel { get; set; } = string.Empty;
        public string RawResponse { get; set; } = string.Empty;
    }

    /// <summary>
    /// AI-generated study plan step per SRS 3.2.2
    /// </summary>
    public class AiStudyPlanStep
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
        public int EstimatedHours { get; set; }
        public List<string> Objectives { get; set; } = new();
        public List<AiStudyPlanCheckpoint> Checkpoints { get; set; } = new();
    }

    /// <summary>
    /// AI-generated checkpoint per SRS 3.2.2
    /// </summary>
    public class AiStudyPlanCheckpoint
    {
        public string Description { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
        public int EstimatedMinutes { get; set; }
        public string Type { get; set; } = string.Empty;
        public bool IsMandatory { get; set; } = true;
    }

    /// <summary>
    /// AI-generated skill requirement per SRS 3.3
    /// </summary>
    public class AiStudyPlanSkill
    {
        public string SkillName { get; set; } = string.Empty;
        public string TargetProficiency { get; set; } = string.Empty;
        public int Importance { get; set; } = 3;
        public bool IsPrerequisite { get; set; } = false;
    }

    /// <summary>
    /// AI-generated learning resource per SRS 3.2.3
    /// </summary>
    public class AiStudyPlanResource
    {
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string ResourceType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? EstimatedHours { get; set; }
        public string? DifficultyLevel { get; set; }
        public bool IsFree { get; set; } = true;
        public decimal? Cost { get; set; }
        public string? Provider { get; set; }
        public int Priority { get; set; } = 3;
        public Guid? StudyPlanStepId { get; set; } // Optional association with specific step
    }
}
