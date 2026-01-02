using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities;
using Mentora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for StudySession entity
    /// </summary>
    public class StudySessionsRepository : IStudySessionsRepository
    {
        private readonly ApplicationDbContext _context;

        public StudySessionsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudySession>> GetUserSessionsAsync(Guid userId, int limit = 50)
        {
            return await _context.StudySessions
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.StartTime)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudySession>> GetSessionsByDateRangeAsync(Guid userId, DateTime from, DateTime to)
        {
            return await _context.StudySessions
                .Where(s => s.UserId == userId && s.StartTime >= from && s.StartTime < to)
                .OrderByDescending(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<StudySession?> GetByIdAsync(Guid id, Guid userId)
        {
            return await _context.StudySessions
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);
        }

        public async Task<StudySession> CreateAsync(StudySession session)
        {
            _context.StudySessions.Add(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            var session = await GetByIdAsync(id, userId);
            
            if (session == null)
                return false;

            _context.StudySessions.Remove(session);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetTotalStudyMinutesAsync(Guid userId)
        {
            return await _context.StudySessions
                .Where(s => s.UserId == userId)
                .SumAsync(s => s.DurationMinutes);
        }
    }
}
