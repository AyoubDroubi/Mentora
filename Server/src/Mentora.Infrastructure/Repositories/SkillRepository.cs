using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities;
using Mentora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Mentora.Infrastructure.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SkillRepository> _logger;

        public SkillRepository(ApplicationDbContext context, ILogger<SkillRepository> logger)
        {
            _context = context;
            _logger = logger;
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

        /// <summary>
        /// Get skill by name or create if not exists per SRS 3.3.2
        /// Ensures shared skill repository for analytics capability
        /// </summary>
        public async Task<Skill> GetOrCreateByNameAsync(string skillName, SkillCategory category)
        {
            // Try to find existing skill (case-insensitive)
            var existing = await _context.Skills
                .FirstOrDefaultAsync(s => s.Name.ToLower() == skillName.ToLower());

            if (existing != null)
            {
                _logger.LogDebug($"Found existing skill: {existing.Name}");
                return existing;
            }

            // Create new skill
            var newSkill = new Skill
            {
                Name = skillName.Trim(),
                Category = category,
                Description = $"Auto-generated skill: {skillName}"
            };

            _context.Skills.Add(newSkill);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"? Created new skill: {newSkill.Name} ({category})");
            return newSkill;
        }
    }
}
