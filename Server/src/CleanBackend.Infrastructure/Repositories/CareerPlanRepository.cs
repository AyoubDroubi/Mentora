using CleanBackend.Application.Interfaces;
using CleanBackend.Domain.Entities;
using CleanBackend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanBackend.Infrastructure.Repositories
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

        // تنفيذ الأوفرلود الثاني من الانترفيس (حذف بوجود الكائن)
        Task ICareerPlanRepository.DeleteAsync(CareerPlan careerPlan)
        {
            _context.CareerPlans.Remove(careerPlan);
            return SaveChangesAsync(); // نرجع Task
        }

        public async Task<List<CareerPlan>> GetAllByUserIdAsync(string userId)
        {
            // نستخدم AsNoTracking لسرعة القراءة فقط
            return await _context.CareerPlans
                .Where(p => p.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<CareerPlan?> GetByIdAndUserIdAsync(int id, string userId)
        {
            return await _context.CareerPlans
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            // يرجع true لو فيه أكثر من صف اتأثر
            return await _context.SaveChangesAsync() > 0;
        }

        // مجرد Implementation للـ Save void if needed
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