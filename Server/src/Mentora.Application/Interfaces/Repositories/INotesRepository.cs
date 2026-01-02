using Mentora.Domain.Entities.StudyPlanner;

namespace Mentora.Application.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for UserNote entity
    /// Handles data access for notes
    /// </summary>
    public interface INotesRepository
    {
        Task<IEnumerable<UserNote>> GetUserNotesAsync(Guid userId);
        Task<UserNote?> GetByIdAsync(Guid id, Guid userId);
        Task<UserNote> CreateAsync(UserNote note);
        Task<UserNote> UpdateAsync(UserNote note);
        Task<bool> DeleteAsync(Guid id, Guid userId);
        Task<int> GetNotesCountAsync(Guid userId);
    }
}
