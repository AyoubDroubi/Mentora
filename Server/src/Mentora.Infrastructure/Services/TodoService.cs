using Mentora.Application.DTOs.StudyPlanner;
using Mentora.Application.Interfaces.Repositories;
using Mentora.Application.Interfaces.Services;
using Mentora.Domain.Entities.StudyPlanner;

namespace Mentora.Infrastructure.Services
{
    /// <summary>
    /// Service implementation for Todo operations
    /// Contains all business logic for todo management
    /// Per SRS Study Planner - Feature 2: ToDo List
    /// </summary>
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;

        public TodoService(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<IEnumerable<TodoItemDto>> GetUserTodosAsync(Guid userId, string filter = "all")
        {
            var todos = await _todoRepository.GetUserTodosAsync(userId, filter);

            return todos.Select(t => new TodoItemDto
            {
                Id = t.Id,
                Title = t.Title,
                IsCompleted = t.IsCompleted,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            });
        }

        public async Task<TodoItemDto?> GetByIdAsync(Guid id, Guid userId)
        {
            var todo = await _todoRepository.GetByIdAsync(id, userId);

            if (todo == null)
                return null;

            return new TodoItemDto
            {
                Id = todo.Id,
                Title = todo.Title,
                IsCompleted = todo.IsCompleted,
                CreatedAt = todo.CreatedAt,
                UpdatedAt = todo.UpdatedAt
            };
        }

        public async Task<TodoItemDto> CreateTodoAsync(Guid userId, CreateTodoDto dto)
        {
            // Business Rule: Title is required and must be trimmed
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Title is required", nameof(dto.Title));

            var todo = new TodoItem
            {
                UserId = userId,
                Title = dto.Title.Trim(),
                IsCompleted = false
            };

            var createdTodo = await _todoRepository.CreateAsync(todo);

            return new TodoItemDto
            {
                Id = createdTodo.Id,
                Title = createdTodo.Title,
                IsCompleted = createdTodo.IsCompleted,
                CreatedAt = createdTodo.CreatedAt,
                UpdatedAt = createdTodo.UpdatedAt
            };
        }

        public async Task<TodoItemDto?> ToggleTodoAsync(Guid id, Guid userId)
        {
            var todo = await _todoRepository.GetByIdAsync(id, userId);

            if (todo == null)
                return null;

            // Business Logic: Toggle completion status
            todo.IsCompleted = !todo.IsCompleted;

            var updatedTodo = await _todoRepository.UpdateAsync(todo);

            return new TodoItemDto
            {
                Id = updatedTodo.Id,
                Title = updatedTodo.Title,
                IsCompleted = updatedTodo.IsCompleted,
                CreatedAt = updatedTodo.CreatedAt,
                UpdatedAt = updatedTodo.UpdatedAt
            };
        }

        public async Task<bool> DeleteTodoAsync(Guid id, Guid userId)
        {
            return await _todoRepository.DeleteAsync(id, userId);
        }

        public async Task<TodoSummaryDto> GetSummaryAsync(Guid userId)
        {
            var totalTasks = await _todoRepository.GetTotalCountAsync(userId);
            var completedTasks = await _todoRepository.GetCompletedCountAsync(userId);
            var pendingTasks = totalTasks - completedTasks;

            var completionRate = totalTasks > 0 
                ? (int)Math.Round((double)completedTasks / totalTasks * 100)
                : 0;

            return new TodoSummaryDto
            {
                TotalTasks = totalTasks,
                CompletedTasks = completedTasks,
                PendingTasks = pendingTasks,
                CompletionRate = completionRate
            };
        }
    }
}
