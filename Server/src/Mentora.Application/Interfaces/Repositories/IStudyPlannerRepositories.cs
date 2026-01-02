using Mentora.Domain.Entities.StudyPlanner;

namespace Mentora.Application.Interfaces.Repositories
{
    public interface IStudyQuizRepository
    {
        Task<StudyQuizAttempt?> GetLatestAttemptAsync(Guid userId);
        Task<StudyQuizAttempt> CreateAsync(StudyQuizAttempt attempt);
    }

    public interface IStudySessionsRepository
    {
        Task<IEnumerable<Domain.Entities.StudySession>> GetUserSessionsAsync(Guid userId, int limit = 50);
        Task<IEnumerable<Domain.Entities.StudySession>> GetSessionsByDateRangeAsync(Guid userId, DateTime from, DateTime to);
        Task<Domain.Entities.StudySession?> GetByIdAsync(Guid id, Guid userId);
        Task<Domain.Entities.StudySession> CreateAsync(Domain.Entities.StudySession session);
        Task<bool> DeleteAsync(Guid id, Guid userId);
        Task<int> GetTotalStudyMinutesAsync(Guid userId);
    }
}
