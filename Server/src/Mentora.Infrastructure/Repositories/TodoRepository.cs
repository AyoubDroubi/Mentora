using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities.StudyPlanner;
using Mentora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for TodoItem entity
    /// Handles all database operations for todos
    /// </summary>
    public class TodoRepository : ITodoRepository
    {
        private readonly ApplicationDbContext _context;

        public TodoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TodoItem>> GetUserTodosAsync(Guid userId, string filter = "all")
        {
            var query = _context.TodoItems.Where(t => t.UserId == userId);

            query = filter.ToLower() switch
            {
                "active" => query.Where(t => !t.IsCompleted),
                "completed" => query.Where(t => t.IsCompleted),
                _ => query
            };

            return await query
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<TodoItem?> GetByIdAsync(Guid id, Guid userId)
        {
            return await _context.TodoItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }

        public async Task<TodoItem> CreateAsync(TodoItem todo)
        {
            _context.TodoItems.Add(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task<TodoItem> UpdateAsync(TodoItem todo)
        {
            _context.TodoItems.Update(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            var todo = await GetByIdAsync(id, userId);
            
            if (todo == null)
                return false;

            _context.TodoItems.Remove(todo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetTotalCountAsync(Guid userId)
        {
            return await _context.TodoItems
                .Where(t => t.UserId == userId)
                .CountAsync();
        }

        public async Task<int> GetCompletedCountAsync(Guid userId)
        {
            return await _context.TodoItems
                .Where(t => t.UserId == userId && t.IsCompleted)
                .CountAsync();
        }
    }
}
