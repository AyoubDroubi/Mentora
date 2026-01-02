using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities;
using Mentora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Infrastructure.Repositories
{
    public class CareerQuizRepository : ICareerQuizRepository
    {
        private readonly ApplicationDbContext _context;

        public CareerQuizRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CareerQuizAttempt> CreateAsync(CareerQuizAttempt quiz)
        {
            _context.CareerQuizAttempts.Add(quiz);
            await _context.SaveChangesAsync();
            return quiz;
        }

        public async Task<CareerQuizAttempt?> GetByIdAsync(Guid id)
        {
            return await _context.CareerQuizAttempts
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<List<CareerQuizAttempt>> GetByUserIdAsync(Guid userId)
        {
            return await _context.CareerQuizAttempts
                .Where(q => q.UserId == userId)
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();
        }

        public async Task<CareerQuizAttempt?> GetLatestByUserIdAsync(Guid userId)
        {
            return await _context.CareerQuizAttempts
                .Where(q => q.UserId == userId)
                .OrderByDescending(q => q.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<CareerQuizAttempt> UpdateAsync(CareerQuizAttempt quiz)
        {
            quiz.UpdatedAt = DateTime.UtcNow;
            _context.CareerQuizAttempts.Update(quiz);
            await _context.SaveChangesAsync();
            return quiz;
        }

        public async Task DeleteAsync(Guid id)
        {
            var quiz = await GetByIdAsync(id);
            if (quiz != null)
            {
                _context.CareerQuizAttempts.Remove(quiz);
                await _context.SaveChangesAsync();
            }
        }
    }
}
