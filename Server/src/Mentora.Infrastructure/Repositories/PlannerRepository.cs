using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities.StudyPlanner;
using Mentora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for PlannerEvent entity
    /// Handles all database operations for events
    /// </summary>
    public class PlannerRepository : IPlannerRepository
    {
        private readonly ApplicationDbContext _context;

        public PlannerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PlannerEvent>> GetUserEventsAsync(Guid userId)
        {
            return await _context.PlannerEvents
                .Where(e => e.UserId == userId)
                .OrderBy(e => e.EventDateTimeUtc)
                .ToListAsync();
        }

        public async Task<IEnumerable<PlannerEvent>> GetEventsByDateAsync(Guid userId, DateTime date)
        {
            var startOfDay = date.Date.ToUniversalTime();
            var endOfDay = startOfDay.AddDays(1);

            return await _context.PlannerEvents
                .Where(e => e.UserId == userId && 
                           e.EventDateTimeUtc >= startOfDay && 
                           e.EventDateTimeUtc < endOfDay)
                .OrderBy(e => e.EventDateTimeUtc)
                .ToListAsync();
        }

        public async Task<IEnumerable<PlannerEvent>> GetUpcomingEventsAsync(Guid userId)
        {
            var now = DateTime.UtcNow;

            return await _context.PlannerEvents
                .Where(e => e.UserId == userId && e.EventDateTimeUtc >= now)
                .OrderBy(e => e.EventDateTimeUtc)
                .ToListAsync();
        }

        public async Task<PlannerEvent?> GetByIdAsync(Guid id, Guid userId)
        {
            return await _context.PlannerEvents
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
        }

        public async Task<PlannerEvent> CreateAsync(PlannerEvent plannerEvent)
        {
            _context.PlannerEvents.Add(plannerEvent);
            await _context.SaveChangesAsync();
            return plannerEvent;
        }

        public async Task<PlannerEvent> UpdateAsync(PlannerEvent plannerEvent)
        {
            _context.PlannerEvents.Update(plannerEvent);
            await _context.SaveChangesAsync();
            return plannerEvent;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            var plannerEvent = await GetByIdAsync(id, userId);
            
            if (plannerEvent == null)
                return false;

            _context.PlannerEvents.Remove(plannerEvent);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetTotalPastEventsCountAsync(Guid userId)
        {
            var now = DateTime.UtcNow;
            return await _context.PlannerEvents
                .Where(e => e.UserId == userId && e.EventDateTimeUtc < now)
                .CountAsync();
        }

        public async Task<int> GetAttendedEventsCountAsync(Guid userId)
        {
            var now = DateTime.UtcNow;
            return await _context.PlannerEvents
                .Where(e => e.UserId == userId && e.EventDateTimeUtc < now && e.IsAttended)
                .CountAsync();
        }
    }
}
