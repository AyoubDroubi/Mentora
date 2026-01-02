using Mentora.Application.DTOs.StudyPlanner;

namespace Mentora.Application.Interfaces.Services
{
    /// <summary>
    /// Service interface for Todo operations
    /// Contains business logic for todo management
    /// </summary>
    public interface ITodoService
    {
        Task<IEnumerable<TodoItemDto>> GetUserTodosAsync(Guid userId, string filter = "all");
        Task<TodoItemDto?> GetByIdAsync(Guid id, Guid userId);
        Task<TodoItemDto> CreateTodoAsync(Guid userId, CreateTodoDto dto);
        Task<TodoItemDto?> ToggleTodoAsync(Guid id, Guid userId);
        Task<bool> DeleteTodoAsync(Guid id, Guid userId);
        Task<TodoSummaryDto> GetSummaryAsync(Guid userId);
    }
}
