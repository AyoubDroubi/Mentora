using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities;
using Mentora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Infrastructure.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly ApplicationDbContext _context;

        public SkillRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Skill> CreateAsync(Skill skill)
        {
            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();
            return skill;
        }

        public async Task<Skill?> GetByIdAsync(Guid id)
        {
            return await _context.Skills.FindAsync(id);
        }

        public async Task<Skill?> GetByNameAsync(string name)
        {
            return await _context.Skills
                .FirstOrDefaultAsync(s => s.Name.ToLower() == name.ToLower());
        }

        public async Task<List<Skill>> GetAllAsync()
        {
            return await _context.Skills
                .OrderBy(s => s.Category)
                .ThenBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<List<Skill>> GetByCategoryAsync(SkillCategory category)
        {
            return await _context.Skills
                .Where(s => s.Category == category)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Skill> UpdateAsync(Skill skill)
        {
            _context.Skills.Update(skill);
            await _context.SaveChangesAsync();
            return skill;
        }

        public async Task DeleteAsync(Guid id)
        {
            var skill = await GetByIdAsync(id);
            if (skill != null)
            {
                _context.Skills.Remove(skill);
                await _context.SaveChangesAsync();
            }
        }
    }
}
