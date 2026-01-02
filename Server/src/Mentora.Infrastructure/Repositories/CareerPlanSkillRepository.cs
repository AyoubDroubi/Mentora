using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities;
using Mentora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Infrastructure.Repositories
{
    public class CareerPlanSkillRepository : ICareerPlanSkillRepository
    {
        private readonly ApplicationDbContext _context;

        public CareerPlanSkillRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CareerPlanSkill> CreateAsync(CareerPlanSkill skill)
        {
            _context.Set<CareerPlanSkill>().Add(skill);
            await _context.SaveChangesAsync();
            return skill;
        }

        public async Task CreateManyAsync(IEnumerable<CareerPlanSkill> skills)
        {
            _context.Set<CareerPlanSkill>().AddRange(skills);
            await _context.SaveChangesAsync();
        }

        public async Task<CareerPlanSkill?> GetByIdAsync(Guid id)
        {
            return await _context.Set<CareerPlanSkill>()
                .Include(s => s.Skill)
                .Include(s => s.CareerStep)
                .Include(s => s.CareerPlan)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<CareerPlanSkill>> GetByPlanIdAsync(Guid planId)
        {
            return await _context.Set<CareerPlanSkill>()
                .Include(s => s.Skill)
                .Include(s => s.CareerStep)
                .Where(s => s.CareerPlanId == planId)
                .OrderBy(s => s.CareerStep != null ? s.CareerStep.OrderIndex : 999)
                .ThenBy(s => s.Skill.Name)
                .ToListAsync();
        }

        public async Task<List<CareerPlanSkill>> GetByStepIdAsync(Guid stepId)
        {
            return await _context.Set<CareerPlanSkill>()
                .Include(s => s.Skill)
                .Where(s => s.CareerStepId == stepId)
                .ToListAsync();
        }

        public async Task<CareerPlanSkill> UpdateAsync(CareerPlanSkill skill)
        {
            _context.Set<CareerPlanSkill>().Update(skill);
            await _context.SaveChangesAsync();
            return skill;
        }

        public async Task DeleteAsync(Guid id)
        {
            var skill = await GetByIdAsync(id);
            if (skill != null)
            {
                _context.Set<CareerPlanSkill>().Remove(skill);
                await _context.SaveChangesAsync();
            }
        }
    }
}
