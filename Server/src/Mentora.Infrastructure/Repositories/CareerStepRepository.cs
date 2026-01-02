using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities;
using Mentora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Infrastructure.Repositories
{
    public class CareerStepRepository : ICareerStepRepository
    {
        private readonly ApplicationDbContext _context;

        public CareerStepRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CareerStep> CreateAsync(CareerStep step)
        {
            _context.CareerSteps.Add(step);
            await _context.SaveChangesAsync();
            return step;
        }

        public async Task CreateManyAsync(IEnumerable<CareerStep> steps)
        {
            _context.CareerSteps.AddRange(steps);
            await _context.SaveChangesAsync();
        }

        public async Task<CareerStep?> GetByIdAsync(Guid id)
        {
            return await _context.CareerSteps
                .Include(s => s.Skills)
                    .ThenInclude(ps => ps.Skill)
                .Include(s => s.LinkedStudyTasks)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<CareerStep>> GetByPlanIdAsync(Guid planId)
        {
            return await _context.CareerSteps
                .Include(s => s.Skills)
                    .ThenInclude(ps => ps.Skill)
                .Where(s => s.CareerPlanId == planId)
                .OrderBy(s => s.OrderIndex)
                .ToListAsync();
        }

        public async Task<CareerStep> UpdateAsync(CareerStep step)
        {
            _context.CareerSteps.Update(step);
            await _context.SaveChangesAsync();
            return step;
        }

        public async Task DeleteAsync(Guid id)
        {
            var step = await _context.CareerSteps.FindAsync(id);
            if (step != null)
            {
                _context.CareerSteps.Remove(step);
                await _context.SaveChangesAsync();
            }
        }
    }
}
