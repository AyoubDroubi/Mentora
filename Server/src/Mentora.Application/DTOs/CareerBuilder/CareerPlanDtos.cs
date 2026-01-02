namespace Mentora.Application.DTOs.CareerBuilder
{
    /// <summary>
    /// DTO for generating a career plan
    /// </summary>
    public class GenerateCareerPlanDto
    {
        public Guid QuizAttemptId { get; set; }
    }

    /// <summary>
    /// Response DTO after generating a career plan
    /// </summary>
    public class CareerPlanGeneratedDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public CareerPlanDetailDto? CareerPlan { get; set; }
    }

    /// <summary>
    /// DTO for career plan list view
    /// </summary>
    public class CareerPlanListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int ProgressPercentage { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// DTO for career plan detail view
    /// </summary>
    public class CareerPlanDetailDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int ProgressPercentage { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CareerStepDto> Steps { get; set; } = new();
    }

    /// <summary>
    /// DTO for career step
    /// </summary>
    public class CareerStepDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
        public int ProgressPercentage { get; set; }
    }

    /// <summary>
    /// DTO for updating career plan status
    /// </summary>
    public class UpdatePlanStatusDto
    {
        public string Status { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO for quiz submission from frontend
    /// </summary>
    public class QuizSubmissionDto
    {
        public Dictionary<string, object> Answers { get; set; } = new();
    }

    /// <summary>
    /// DTO for updating skill status
    /// </summary>
    public class UpdateSkillStatusDto
    {
        public string Status { get; set; } = string.Empty;
    }
}
