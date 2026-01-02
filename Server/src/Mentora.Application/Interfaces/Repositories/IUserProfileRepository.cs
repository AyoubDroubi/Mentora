using Mentora.Domain.Entities;

namespace Mentora.Application.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for UserProfile entity
    /// Handles data access for user profile management
    /// </summary>
    public interface IUserProfileRepository
    {
        Task<UserProfile?> GetByUserIdAsync(Guid userId);
        Task<UserProfile?> GetByIdAsync(Guid id);
        Task<UserProfile> CreateAsync(UserProfile profile);
        Task<UserProfile> UpdateAsync(UserProfile profile);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid userId);
        Task<int> CountAsync();
    }
}
