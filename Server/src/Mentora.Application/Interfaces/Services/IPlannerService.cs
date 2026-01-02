using Mentora.Application.DTOs.StudyPlanner;

namespace Mentora.Application.Interfaces.Services
{
    /// <summary>
    /// Service interface for Planner operations
    /// Contains business logic for event management
    /// </summary>
    public interface IPlannerService
    {
        Task<IEnumerable<PlannerEventDto>> GetUserEventsAsync(Guid userId);
        Task<IEnumerable<PlannerEventDto>> GetEventsByDateAsync(Guid userId, string date);
        Task<IEnumerable<PlannerEventDto>> GetUpcomingEventsAsync(Guid userId);
        Task<PlannerEventDto?> GetByIdAsync(Guid id, Guid userId);
        Task<PlannerEventDto> CreateEventAsync(Guid userId, CreateEventDto dto);
        Task<PlannerEventDto?> MarkAttendedAsync(Guid id, Guid userId);
        Task<bool> DeleteEventAsync(Guid id, Guid userId);
    }
}
