using Mentora.Application.DTOs.UserProfile;

namespace Mentora.Application.Interfaces.Services
{
    /// <summary>
    /// Service interface for UserProfileSkill operations
    /// Per SRS 2.3: Skills Portfolio Management
    /// </summary>
    public interface IUserProfileSkillService
    {
        // CRUD Operations per SRS 2.3.2
        
        Task<IEnumerable<UserProfileSkillDto>> GetUserSkillsAsync(
            Guid userId,
            int? proficiencyLevel = null,
            bool? isFeatured = null,
            string? sortBy = null);
        
        Task<UserProfileSkillDto?> GetSkillByIdAsync(Guid userId, Guid skillId);
        
        Task<IEnumerable<UserProfileSkillDto>> GetFeaturedSkillsAsync(Guid userId);
        
        Task<UserProfileSkillDto> AddSkillAsync(Guid userId, AddSkillDto dto);
        
        Task<BulkOperationResultDto> AddSkillsAsync(Guid userId, AddBulkSkillsDto dto);
        
        Task<UserProfileSkillDto> UpdateSkillAsync(Guid userId, Guid skillId, UpdateSkillDto dto);
        
        Task<bool> DeleteSkillAsync(Guid userId, Guid skillId);
        
        Task<BulkOperationResultDto> DeleteSkillsAsync(Guid userId, DeleteBulkSkillsDto dto);
        
        Task<bool> ToggleFeaturedAsync(Guid userId, Guid skillId);
        
        Task<bool> ReorderSkillsAsync(Guid userId, ReorderSkillsDto dto);
        
        // Analytics per SRS 2.3.3
        
        Task<SkillsSummaryDto> GetSkillsSummaryAsync(Guid userId);
        
        Task<SkillsDistributionDto> GetSkillsDistributionAsync(Guid userId);
        
        Task<SkillsTimelineDto> GetSkillsTimelineAsync(Guid userId);
        
        Task<SkillsCoverageDto> GetSkillsCoverageAsync(Guid userId);
    }
}
