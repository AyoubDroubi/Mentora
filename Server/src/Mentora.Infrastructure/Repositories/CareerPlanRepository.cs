using CleanBackend.Domain.Entities.Auth;
using Mentora.Application.Interfaces;
using Mentora.Domain.Entities;
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

        public async Task<bool> DeleteAsync(CareerPlan careerPlan)
        {
            _context.CareerPlans.Remove(careerPlan);
            await SaveChangesAsync();
            return true;
        }

        Task ICareerPlanRepository.DeleteAsync(CareerPlan careerPlan)
        {
            _context.CareerPlans.Remove(careerPlan);
            return SaveChangesAsync(); // نرجع Task
        }

        public async Task<List<CareerPlan>> GetAllByUserIdAsync(Guid userId)
        {
            return await _context.CareerPlans
                .Where(p => p.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<CareerPlan?> GetByIdAndUserIdAsync(Guid id, Guid userId)
        {
            return await _context.CareerPlans
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        async Task<bool> ICareerPlanRepository.SaveChangesAsync()
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