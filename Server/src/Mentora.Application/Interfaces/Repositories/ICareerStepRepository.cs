using Mentora.Domain.Entities;

namespace Mentora.Application.Interfaces.Repositories
{
    public interface ICareerStepRepository
    {
        Task<CareerStep> CreateAsync(CareerStep step);
        Task CreateManyAsync(IEnumerable<CareerStep> steps);
        Task<List<CareerStep>> GetByPlanIdAsync(Guid planId);
        Task<CareerStep?> GetByIdAsync(Guid id);
        Task<CareerStep> UpdateAsync(CareerStep step);
        Task DeleteAsync(Guid id);
    }
}
