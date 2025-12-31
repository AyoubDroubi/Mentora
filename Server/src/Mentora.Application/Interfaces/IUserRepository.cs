using Mentora.Domain.Entities.Auth;

namespace Mentora.Application.Interfaces
{
    /// <summary>
    /// User repository interface per SRS 1.1.2: Uniqueness Check
    /// </summary>
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid userId);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> ExistsByEmailAsync(string email);
        Task<User> CreateAsync(User user);
        Task UpdateAsync(User user);
        Task<IEnumerable<User>> GetAllAsync();
    }
}
