using System.ComponentModel.DataAnnotations;

namespace Mentora.Application.DTOs.UserProfile
{
    /// <summary>
    /// DTO for adding a skill to user profile
    /// Per SRS 2.3.2.1: Add Skills
    /// </summary>
    public class AddSkillDto
    {
        [Required(ErrorMessage = "Skill ID is required")]
        public Guid SkillId { get; set; }

        [Range(0, 3, ErrorMessage = "Proficiency level must be between 0-3 (Beginner to Expert)")]
        public int ProficiencyLevel { get; set; } = 0;

        [MaxLength(200, ErrorMessage = "Acquisition method cannot exceed 200 characters")]
        public string? AcquisitionMethod { get; set; }

        public DateTime? StartedDate { get; set; }

        [Range(0, 50, ErrorMessage = "Years of experience must be between 0-50")]
        public int? YearsOfExperience { get; set; }

        public bool IsFeatured { get; set; } = false;

        [MaxLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
        public string? Notes { get; set; }

        public int DisplayOrder { get; set; } = 0;
    }

    /// <summary>
    /// DTO for bulk adding skills
    /// Per SRS 2.3.2.1: Add Bulk Skills
    /// </summary>
    public class AddBulkSkillsDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "At least one skill is required")]
        [MaxLength(50, ErrorMessage = "Cannot add more than 50 skills at once")]
        public List<AddSkillDto> Skills { get; set; } = new();
    }

    /// <summary>
    /// DTO for updating a skill
    /// Per SRS 2.3.2.3: Update Skills
    /// </summary>
    public class UpdateSkillDto
    {
        [Range(0, 3, ErrorMessage = "Proficiency level must be between 0-3")]
        public int? ProficiencyLevel { get; set; }

        [MaxLength(200)]
        public string? AcquisitionMethod { get; set; }

        public DateTime? StartedDate { get; set; }

        [Range(0, 50)]
        public int? YearsOfExperience { get; set; }

        public bool? IsFeatured { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        public int? DisplayOrder { get; set; }
    }

    /// <summary>
    /// DTO for skill response
    /// </summary>
    public class UserProfileSkillDto
    {
        public Guid Id { get; set; }
        public Guid UserProfileId { get; set; }
        public Guid SkillId { get; set; }
        public string SkillName { get; set; } = string.Empty;
        public string SkillCategory { get; set; } = string.Empty;
        public int ProficiencyLevel { get; set; }
        public string ProficiencyLevelName { get; set; } = string.Empty;
        public string? AcquisitionMethod { get; set; }
        public DateTime? StartedDate { get; set; }
        public int? YearsOfExperience { get; set; }
        public bool IsFeatured { get; set; }
        public string? Notes { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// DTO for reordering featured skills
    /// Per SRS 2.3.6.1: Display Order Management
    /// </summary>
    public class ReorderSkillsDto
    {
        [Required]
        [MinLength(1)]
        public List<SkillOrderDto> SkillOrders { get; set; } = new();
    }

    public class SkillOrderDto
    {
        [Required]
        public Guid SkillId { get; set; }

        [Range(0, int.MaxValue)]
        public int DisplayOrder { get; set; }
    }

    /// <summary>
    /// DTO for bulk delete
    /// Per SRS 2.3.2.4: Delete Bulk Skills
    /// </summary>
    public class DeleteBulkSkillsDto
    {
        [Required]
        [MinLength(1)]
        public List<Guid> SkillIds { get; set; } = new();
    }

    /// <summary>
    /// DTO for skills summary statistics
    /// Per SRS 2.3.3.1: Skills Summary
    /// </summary>
    public class SkillsSummaryDto
    {
        public int TotalSkills { get; set; }
        public Dictionary<string, int> ProficiencyDistribution { get; set; } = new();
        public Dictionary<string, int> CategoryBreakdown { get; set; } = new();
        public int TotalExperienceYears { get; set; }
        public int FeaturedSkillsCount { get; set; }
    }

    /// <summary>
    /// DTO for skills distribution analytics
    /// Per SRS 2.3.3.2: Skills Distribution
    /// </summary>
    public class SkillsDistributionDto
    {
        public Dictionary<string, double> ByProficiency { get; set; } = new();
        public Dictionary<string, double> ByCategory { get; set; } = new();
    }

    /// <summary>
    /// DTO for skills timeline
    /// Per SRS 2.3.3.3: Skills Timeline
    /// </summary>
    public class SkillsTimelineDto
    {
        public List<TimelineYearDto> Timeline { get; set; } = new();
    }

    public class TimelineYearDto
    {
        public int Year { get; set; }
        public List<TimelineMonthDto> Months { get; set; } = new();
    }

    public class TimelineMonthDto
    {
        public int Month { get; set; }
        public List<TimelineSkillDto> Skills { get; set; } = new();
    }

    public class TimelineSkillDto
    {
        public string Name { get; set; } = string.Empty;
        public string Proficiency { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO for skills coverage analysis
    /// Per SRS 2.3.3.4: Skills Coverage Analysis
    /// </summary>
    public class SkillsCoverageDto
    {
        public int CoverageScore { get; set; }
        public List<string> StrongCategories { get; set; } = new();
        public List<string> WeakCategories { get; set; } = new();
        public List<string> Recommendations { get; set; } = new();
        public Dictionary<string, CategoryCoverageDto> GapAnalysis { get; set; } = new();
    }

    public class CategoryCoverageDto
    {
        public int Coverage { get; set; }
        public List<string> Missing { get; set; } = new();
    }

    /// <summary>
    /// DTO for bulk operation result
    /// Per SRS 9.3: Bulk Operations
    /// </summary>
    public class BulkOperationResultDto
    {
        public bool Success { get; set; }
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }
        public List<BulkOperationItemDto> Results { get; set; } = new();
    }

    public class BulkOperationItemDto
    {
        public Guid? ItemId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Reason { get; set; }
    }
}
