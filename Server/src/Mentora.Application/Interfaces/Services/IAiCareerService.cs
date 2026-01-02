namespace Mentora.Application.Interfaces.Services
{
    /// <summary>
    /// AI Service interface for generating career plans per SRS Section 7
    /// </summary>
    public interface IAiCareerService
    {
        /// <summary>
        /// Generate career plan using AI based on quiz answers
        /// Returns structured career plan data
        /// </summary>
        Task<AiCareerPlanResponse> GenerateCareerPlanAsync(string quizAnswersJson);
    }

    /// <summary>
    /// AI response structure for career plan generation
    /// </summary>
    public class AiCareerPlanResponse
    {
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public List<AiCareerStep> Steps { get; set; } = new();
        public List<AiCareerSkill> Skills { get; set; } = new();
        
        // Metadata
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public string AiModel { get; set; } = string.Empty; // e.g., "gpt-4", "gpt-3.5-turbo"
    }

    public class AiCareerStep
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
        public List<string> SkillNames { get; set; } = new(); // Skills required for this step
    }

    public class AiCareerSkill
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // Technical or Soft
    }
}
