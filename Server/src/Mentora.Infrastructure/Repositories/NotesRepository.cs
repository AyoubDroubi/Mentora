using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities.StudyPlanner;
using Mentora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for UserNote entity
    /// Handles all database operations for notes
    /// </summary>
    public class NotesRepository : INotesRepository
    {
        private readonly ApplicationDbContext _context;

        public NotesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserNote>> GetUserNotesAsync(Guid userId)
        {
            return await _context.UserNotes
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<UserNote?> GetByIdAsync(Guid id, Guid userId)
        {
            return await _context.UserNotes
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
        }

        public async Task<UserNote> CreateAsync(UserNote note)
        {
            _context.UserNotes.Add(note);
            await _context.SaveChangesAsync();
            return note;
        }

        public async Task<UserNote> UpdateAsync(UserNote note)
        {
            _context.UserNotes.Update(note);
            await _context.SaveChangesAsync();
            return note;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            var note = await GetByIdAsync(id, userId);
            
            if (note == null)
                return false;

            _context.UserNotes.Remove(note);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetNotesCountAsync(Guid userId)
        {
            return await _context.UserNotes
                .Where(n => n.UserId == userId)
                .CountAsync();
        }
    }
}
