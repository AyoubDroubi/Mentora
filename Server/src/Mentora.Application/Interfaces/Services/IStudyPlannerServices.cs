using Mentora.Application.DTOs.StudyPlanner;

namespace Mentora.Application.Interfaces.Services
{
    public interface IStudyQuizService
    {
        Task<IEnumerable<object>> GetQuestionsAsync();
        Task<StudyQuizResultDto> SubmitQuizAsync(Guid userId, Dictionary<string, string> answers);
        Task<StudyQuizResultDto?> GetLatestAttemptAsync(Guid userId);
    }

    public interface IStudySessionsService
    {
        Task<StudySessionDto> SaveSessionAsync(Guid userId, SaveSessionDto dto);
        Task<StudySessionSummaryDto> GetSummaryAsync(Guid userId);
        Task<IEnumerable<StudySessionDto>> GetSessionsAsync(Guid userId, int limit = 50);
        Task<SessionRangeResultDto> GetSessionsByRangeAsync(Guid userId, string from, string to);
        Task<bool> DeleteSessionAsync(Guid id, Guid userId);
    }

    public interface IAttendanceService
    {
        Task<AttendanceSummaryDto> GetSummaryAsync(Guid userId);
        Task<AttendanceHistoryDto> GetHistoryAsync(Guid userId, int days = 30);
        Task<WeeklyProgressDto> GetWeeklyProgressAsync(Guid userId);
    }
}
