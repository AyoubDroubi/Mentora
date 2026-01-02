using Mentora.Application.DTOs.StudyPlanner;
using Mentora.Application.Interfaces.Repositories;
using Mentora.Application.Interfaces.Services;
using Mentora.Domain.Entities.StudyPlanner;

namespace Mentora.Infrastructure.Services
{
    /// <summary>
    /// Service implementation for Planner operations
    /// Contains all business logic for event management
    /// Per SRS Study Planner - Feature 3: Planner (Calendar & Events)
    /// </summary>
    public class PlannerService : IPlannerService
    {
        private readonly IPlannerRepository _plannerRepository;

        public PlannerService(IPlannerRepository plannerRepository)
        {
            _plannerRepository = plannerRepository;
        }

        public async Task<IEnumerable<PlannerEventDto>> GetUserEventsAsync(Guid userId)
        {
            var events = await _plannerRepository.GetUserEventsAsync(userId);
            return MapToDto(events);
        }

        public async Task<IEnumerable<PlannerEventDto>> GetEventsByDateAsync(Guid userId, string date)
        {
            if (!DateTime.TryParse(date, out var targetDate))
            {
                throw new ArgumentException("Invalid date format", nameof(date));
            }

            var events = await _plannerRepository.GetEventsByDateAsync(userId, targetDate);
            return MapToDto(events);
        }

        public async Task<IEnumerable<PlannerEventDto>> GetUpcomingEventsAsync(Guid userId)
        {
            var events = await _plannerRepository.GetUpcomingEventsAsync(userId);
            return MapToDto(events);
        }

        public async Task<PlannerEventDto?> GetByIdAsync(Guid id, Guid userId)
        {
            var plannerEvent = await _plannerRepository.GetByIdAsync(id, userId);
            return plannerEvent == null ? null : MapToDto(plannerEvent);
        }

        public async Task<PlannerEventDto> CreateEventAsync(Guid userId, CreateEventDto dto)
        {
            // Business Rule: Title is required
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Title is required", nameof(dto.Title));

            // Business Rule: Event date must be valid
            if (dto.EventDateTime == default)
                throw new ArgumentException("Event date is required", nameof(dto.EventDateTime));

            var plannerEvent = new PlannerEvent
            {
                UserId = userId,
                Title = dto.Title.Trim(),
                EventDateTimeUtc = dto.EventDateTime.ToUniversalTime(),
                IsAttended = false
            };

            var createdEvent = await _plannerRepository.CreateAsync(plannerEvent);
            return MapToDto(createdEvent);
        }

        public async Task<PlannerEventDto?> MarkAttendedAsync(Guid id, Guid userId)
        {
            var plannerEvent = await _plannerRepository.GetByIdAsync(id, userId);

            if (plannerEvent == null)
                return null;

            // Business Logic: Mark as attended
            plannerEvent.IsAttended = true;

            var updatedEvent = await _plannerRepository.UpdateAsync(plannerEvent);
            return MapToDto(updatedEvent);
        }

        public async Task<bool> DeleteEventAsync(Guid id, Guid userId)
        {
            return await _plannerRepository.DeleteAsync(id, userId);
        }

        private PlannerEventDto MapToDto(PlannerEvent plannerEvent)
        {
            return new PlannerEventDto
            {
                Id = plannerEvent.Id,
                Title = plannerEvent.Title,
                EventDateTime = plannerEvent.EventDateTimeUtc,
                IsAttended = plannerEvent.IsAttended,
                CreatedAt = plannerEvent.CreatedAt
            };
        }

        private IEnumerable<PlannerEventDto> MapToDto(IEnumerable<PlannerEvent> events)
        {
            return events.Select(MapToDto);
        }
    }
}
