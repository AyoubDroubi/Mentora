using Mentora.Application.DTOs.StudyPlanner;
using Mentora.Application.Interfaces.Repositories;
using Mentora.Application.Interfaces.Services;
using Mentora.Domain.Entities;

namespace Mentora.Infrastructure.Services
{
    /// <summary>
    /// Service implementation for Study Sessions operations
    /// Per SRS Study Planner - Feature 1: Pomodoro Timer (Study Sessions)
    /// </summary>
    public class StudySessionsService : IStudySessionsService
    {
        private readonly IStudySessionsRepository _sessionsRepository;

        public StudySessionsService(IStudySessionsRepository sessionsRepository)
        {
            _sessionsRepository = sessionsRepository;
        }

        public async Task<StudySessionDto> SaveSessionAsync(Guid userId, SaveSessionDto dto)
        {
            // Business Rule: Duration must be positive
            if (dto.DurationMinutes <= 0)
                throw new ArgumentException("Duration must be greater than 0", nameof(dto.DurationMinutes));

            var session = new StudySession
            {
                UserId = userId,
                DurationMinutes = dto.DurationMinutes,
                StartTime = dto.StartTime ?? DateTime.UtcNow,
                EndTime = DateTime.UtcNow,
                PauseCount = dto.PauseCount,
                FocusScore = dto.FocusScore
            };

            var savedSession = await _sessionsRepository.CreateAsync(session);

            return MapToDto(savedSession);
        }

        public async Task<StudySessionSummaryDto> GetSummaryAsync(Guid userId)
        {
            var totalMinutes = await _sessionsRepository.GetTotalStudyMinutesAsync(userId);
            var hours = totalMinutes / 60;
            var minutes = totalMinutes % 60;

            return new StudySessionSummaryDto
            {
                TotalMinutes = totalMinutes,
                Hours = hours,
                Minutes = minutes,
                Formatted = $"{hours}h {minutes}m"
            };
        }

        public async Task<IEnumerable<StudySessionDto>> GetSessionsAsync(Guid userId, int limit = 50)
        {
            var sessions = await _sessionsRepository.GetUserSessionsAsync(userId, limit);
            return sessions.Select(MapToDto);
        }

        public async Task<SessionRangeResultDto> GetSessionsByRangeAsync(Guid userId, string from, string to)
        {
            if (!DateTime.TryParse(from, out var fromDate) || !DateTime.TryParse(to, out var toDate))
            {
                throw new ArgumentException("Invalid date format");
            }

            var fromUtc = fromDate.Date.ToUniversalTime();
            var toUtc = toDate.Date.AddDays(1).ToUniversalTime();

            var sessions = await _sessionsRepository.GetSessionsByDateRangeAsync(userId, fromUtc, toUtc);
            var sessionDtos = sessions.Select(MapToDto).ToList();

            var totalMinutes = sessionDtos.Sum(s => s.DurationMinutes);
            var hours = totalMinutes / 60;
            var minutes = totalMinutes % 60;

            return new SessionRangeResultDto
            {
                Sessions = sessionDtos,
                Summary = new StudySessionSummaryDto
                {
                    TotalMinutes = totalMinutes,
                    Hours = hours,
                    Minutes = minutes,
                    Formatted = $"{hours}h {minutes}m"
                }
            };
        }

        public async Task<bool> DeleteSessionAsync(Guid id, Guid userId)
        {
            return await _sessionsRepository.DeleteAsync(id, userId);
        }

        private StudySessionDto MapToDto(StudySession session)
        {
            return new StudySessionDto
            {
                Id = session.Id,
                DurationMinutes = session.DurationMinutes,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                PauseCount = session.PauseCount,
                FocusScore = session.FocusScore,
                CreatedAt = session.CreatedAt
            };
        }
    }
}
