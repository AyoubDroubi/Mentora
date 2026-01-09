using Mentora.Domain.Entities;

namespace Mentora.Application.Interfaces.Repositories
{
    public interface ISkillRepository
    {
        Task<Skill> CreateAsync(Skill skill);
        Task<Skill?> GetByIdAsync(Guid id);
        Task<Skill?> GetByNameAsync(string name);
        Task<List<Skill>> GetAllAsync();
        Task<List<Skill>> GetByCategoryAsync(SkillCategory category);
        Task<Skill> UpdateAsync(Skill skill);
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// Get skill by name or create if not exists per SRS 3.3.2
        /// </summary>
        Task<Skill> GetOrCreateByNameAsync(string skillName, SkillCategory category);
    }
}
