using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities.StudyPlanner;
using Mentora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for StudyQuizAttempt entity
    /// </summary>
    public class StudyQuizRepository : IStudyQuizRepository
    {
        private readonly ApplicationDbContext _context;

        public StudyQuizRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StudyQuizAttempt?> GetLatestAttemptAsync(Guid userId)
        {
            return await _context.StudyQuizAttempts
                .Where(q => q.UserId == userId)
                .OrderByDescending(q => q.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<StudyQuizAttempt> CreateAsync(StudyQuizAttempt attempt)
        {
            _context.StudyQuizAttempts.Add(attempt);
            await _context.SaveChangesAsync();
            return attempt;
        }
    }
}
