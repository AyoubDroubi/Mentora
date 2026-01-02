using Mentora.Domain.Entities.StudyPlanner;

namespace Mentora.Application.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for TodoItem entity
    /// Handles data access for todo items
    /// </summary>
    public interface ITodoRepository
    {
        Task<IEnumerable<TodoItem>> GetUserTodosAsync(Guid userId, string filter = "all");
        Task<TodoItem?> GetByIdAsync(Guid id, Guid userId);
        Task<TodoItem> CreateAsync(TodoItem todo);
        Task<TodoItem> UpdateAsync(TodoItem todo);
        Task<bool> DeleteAsync(Guid id, Guid userId);
        Task<int> GetTotalCountAsync(Guid userId);
        Task<int> GetCompletedCountAsync(Guid userId);
    }
}
