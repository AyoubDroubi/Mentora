using Mentora.Domain.Entities.StudyPlanner;

namespace Mentora.Application.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for PlannerEvent entity
    /// Handles data access for calendar events
    /// </summary>
    public interface IPlannerRepository
    {
        Task<IEnumerable<PlannerEvent>> GetUserEventsAsync(Guid userId);
        Task<IEnumerable<PlannerEvent>> GetEventsByDateAsync(Guid userId, DateTime date);
        Task<IEnumerable<PlannerEvent>> GetUpcomingEventsAsync(Guid userId);
        Task<PlannerEvent?> GetByIdAsync(Guid id, Guid userId);
        Task<PlannerEvent> CreateAsync(PlannerEvent plannerEvent);
        Task<PlannerEvent> UpdateAsync(PlannerEvent plannerEvent);
        Task<bool> DeleteAsync(Guid id, Guid userId);
        Task<int> GetTotalPastEventsCountAsync(Guid userId);
        Task<int> GetAttendedEventsCountAsync(Guid userId);
    }
}
