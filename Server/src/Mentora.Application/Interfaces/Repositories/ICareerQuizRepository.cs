using Mentora.Domain.Entities;

namespace Mentora.Application.Interfaces.Repositories
{
    public interface ICareerQuizRepository
    {
        Task<CareerQuizAttempt> CreateAsync(CareerQuizAttempt quiz);
        Task<CareerQuizAttempt?> GetByIdAsync(Guid id);
        Task<List<CareerQuizAttempt>> GetByUserIdAsync(Guid userId);
        Task<CareerQuizAttempt?> GetLatestByUserIdAsync(Guid userId);
        Task<CareerQuizAttempt> UpdateAsync(CareerQuizAttempt quiz);
        Task DeleteAsync(Guid id);
    }
}
