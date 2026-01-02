using Mentora.Domain.Entities;
using Mentora.Application.Interfaces.Repositories;
using Mentora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Infrastructure.Repositories
{
    public class CareerPlanRepository : ICareerPlanRepository
    {
        private readonly ApplicationDbContext _context;

        public CareerPlanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CareerPlan> CreateAsync(CareerPlan careerPlan)
        {
            _context.CareerPlans.Add(careerPlan);
            await SaveChangesAsync();
            return careerPlan;
        }

        public async Task DeleteAsync(CareerPlan careerPlan)
        {
            _context.CareerPlans.Remove(careerPlan);
            await SaveChangesAsync();
        }

        public async Task<List<CareerPlan>> GetAllByUserIdAsync(Guid userId)
        {
            return await _context.CareerPlans
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<CareerPlan?> GetByIdAndUserIdAsync(Guid id, Guid userId)
        {
            return await _context.CareerPlans
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
        }

        public async Task<CareerPlan?> GetByIdWithStepsAsync(Guid id)
        {
            return await _context.CareerPlans
                .Include(p => p.Steps.OrderBy(s => s.OrderIndex))
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task UpdateAsync(CareerPlan careerPlan)
        {
            _context.CareerPlans.Update(careerPlan);
            await SaveChangesAsync();
        }
    }
}