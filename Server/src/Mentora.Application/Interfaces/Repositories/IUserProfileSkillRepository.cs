using Mentora.Domain.Entities;

namespace Mentora.Application.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for UserProfileSkill entity
    /// Per SRS 2.3.2: Skills CRUD Operations
    /// </summary>
    public interface IUserProfileSkillRepository
    {
        // Basic CRUD Operations per SRS 2.3.2.1-2.3.2.4
        
        /// <summary>
        /// Get all skills for a user profile with optional filtering
        /// Per SRS 2.3.2.2: Retrieve Skills
        /// </summary>
        Task<IEnumerable<UserProfileSkill>> GetUserSkillsAsync(
            Guid userProfileId,
            int? proficiencyLevel = null,
            bool? isFeatured = null,
            string? sortBy = null);
        
        /// <summary>
        /// Get a specific skill by ID
        /// </summary>
        Task<UserProfileSkill?> GetByIdAsync(Guid id);
        
        /// <summary>
        /// Check if skill already exists for user profile
        /// Per SRS 2.3.8.4: Duplicate Prevention
        /// </summary>
        Task<bool> SkillExistsAsync(Guid userProfileId, Guid skillId);
        
        /// <summary>
        /// Get featured skills for a user profile, sorted by DisplayOrder
        /// Per SRS 2.3.6.3: Featured Retrieval
        /// </summary>
        Task<IEnumerable<UserProfileSkill>> GetFeaturedSkillsAsync(Guid userProfileId);
        
        /// <summary>
        /// Add a skill to user profile
        /// Per SRS 2.3.2.1: Add Skills
        /// </summary>
        Task<UserProfileSkill> AddSkillAsync(UserProfileSkill skill);
        
        /// <summary>
        /// Add multiple skills in bulk
        /// Per SRS 2.3.2.1: Add Bulk Skills
        /// </summary>
        Task<IEnumerable<UserProfileSkill>> AddSkillsAsync(IEnumerable<UserProfileSkill> skills);
        
        /// <summary>
        /// Update a skill
        /// Per SRS 2.3.2.3: Update Skills
        /// </summary>
        Task<UserProfileSkill> UpdateSkillAsync(UserProfileSkill skill);
        
        /// <summary>
        /// Delete a skill
        /// Per SRS 2.3.2.4: Delete Skills
        /// </summary>
        Task<bool> DeleteSkillAsync(Guid id);
        
        /// <summary>
        /// Delete multiple skills in bulk
        /// Per SRS 2.3.2.4: Delete Bulk Skills
        /// </summary>
        Task<int> DeleteSkillsAsync(IEnumerable<Guid> ids);
        
        // Analytics Methods per SRS 2.3.3
        
        /// <summary>
        /// Get total skills count for a user profile
        /// Per SRS 2.3.1.3: Business Rules - Maximum 100 skills
        /// </summary>
        Task<int> GetSkillsCountAsync(Guid userProfileId);
        
        /// <summary>
        /// Get featured skills count for a user profile
        /// Per SRS 2.3.1.3: Business Rules - Maximum 10 featured skills
        /// </summary>
        Task<int> GetFeaturedCountAsync(Guid userProfileId);
        
        /// <summary>
        /// Get skills grouped by proficiency level
        /// Per SRS 2.3.3.2: Skills Distribution
        /// </summary>
        Task<Dictionary<int, int>> GetSkillsByProficiencyAsync(Guid userProfileId);
        
        /// <summary>
        /// Get skills grouped by category
        /// Per SRS 2.3.3.1: Skills Summary - Category Breakdown
        /// </summary>
        Task<Dictionary<string, int>> GetSkillsByCategoryAsync(Guid userProfileId);
        
        /// <summary>
        /// Get skills timeline (grouped by year and month)
        /// Per SRS 2.3.3.3: Skills Timeline
        /// </summary>
        Task<IEnumerable<UserProfileSkill>> GetSkillsTimelineAsync(Guid userProfileId);
        
        /// <summary>
        /// Get total years of experience across all skills
        /// Per SRS 2.3.3.1: Skills Summary - Total Experience Years
        /// </summary>
        Task<int> GetTotalExperienceYearsAsync(Guid userProfileId);
    }
}
