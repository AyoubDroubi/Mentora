using Mentora.Domain.Entities;

namespace Mentora.Application.Interfaces.Repositories
{
    public interface ICareerPlanRepository
    {
        Task<CareerPlan> CreateAsync(CareerPlan careerPlan);

        Task<List<CareerPlan>> GetAllByUserIdAsync(Guid userId);

        Task<CareerPlan?> GetByIdAndUserIdAsync(Guid id, Guid userId);

        Task<CareerPlan?> GetByIdWithStepsAsync(Guid id);

        Task UpdateAsync(CareerPlan careerPlan);

        Task DeleteAsync(CareerPlan careerPlan);

        Task<bool> SaveChangesAsync();
    }
}