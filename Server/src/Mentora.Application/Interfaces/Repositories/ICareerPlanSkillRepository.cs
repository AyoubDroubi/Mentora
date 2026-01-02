using Mentora.Domain.Entities;

namespace Mentora.Application.Interfaces.Repositories
{
    public interface ICareerPlanSkillRepository
    {
        Task<CareerPlanSkill> CreateAsync(CareerPlanSkill skill);
        Task CreateManyAsync(IEnumerable<CareerPlanSkill> skills);
        Task<List<CareerPlanSkill>> GetByPlanIdAsync(Guid planId);
        Task<List<CareerPlanSkill>> GetByStepIdAsync(Guid stepId);
        Task<CareerPlanSkill?> GetByIdAsync(Guid id);
        Task<CareerPlanSkill> UpdateAsync(CareerPlanSkill skill);
        Task DeleteAsync(Guid id);
    }
}
