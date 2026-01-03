using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities;
using Mentora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for UserProfileSkill entity
    /// Per SRS 2.3.2: Skills CRUD Operations
    /// </summary>
    public class UserProfileSkillRepository : IUserProfileSkillRepository
    {
        private readonly ApplicationDbContext _context;

        public UserProfileSkillRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserProfileSkill>> GetUserSkillsAsync(
            Guid userProfileId,
            int? proficiencyLevel = null,
            bool? isFeatured = null,
            string? sortBy = null)
        {
            var query = _context.UserProfileSkills
                .Include(ups => ups.Skill)
                .Where(ups => ups.UserProfileId == userProfileId);

            // Filtering per SRS 2.3.2.2
            if (proficiencyLevel.HasValue)
            {
                query = query.Where(ups => ups.ProficiencyLevel == proficiencyLevel.Value);
            }

            if (isFeatured.HasValue)
            {
                query = query.Where(ups => ups.IsFeatured == isFeatured.Value);
            }

            // Sorting
            query = sortBy?.ToLower() switch
            {
                "proficiency" => query.OrderByDescending(ups => ups.ProficiencyLevel),
                "experience" => query.OrderByDescending(ups => ups.YearsOfExperience),
                "date" => query.OrderByDescending(ups => ups.StartedDate),
                "name" => query.OrderBy(ups => ups.Skill.Name),
                _ => query.OrderBy(ups => ups.Skill.Name)
            };

            return await query.ToListAsync();
        }

        public async Task<UserProfileSkill?> GetByIdAsync(Guid id)
        {
            return await _context.UserProfileSkills
                .Include(ups => ups.Skill)
                .Include(ups => ups.UserProfile)
                .FirstOrDefaultAsync(ups => ups.Id == id);
        }

        public async Task<bool> SkillExistsAsync(Guid userProfileId, Guid skillId)
        {
            // Per SRS 2.3.8.4: Duplicate Prevention
            return await _context.UserProfileSkills
                .AnyAsync(ups => ups.UserProfileId == userProfileId && ups.SkillId == skillId);
        }

        public async Task<IEnumerable<UserProfileSkill>> GetFeaturedSkillsAsync(Guid userProfileId)
        {
            // Per SRS 2.3.6.3: Featured Retrieval
            return await _context.UserProfileSkills
                .Include(ups => ups.Skill)
                .Where(ups => ups.UserProfileId == userProfileId && ups.IsFeatured)
                .OrderBy(ups => ups.DisplayOrder)
                .ToListAsync();
        }

        public async Task<UserProfileSkill> AddSkillAsync(UserProfileSkill skill)
        {
            // Per SRS 2.3.2.1: Add Skills
            _context.UserProfileSkills.Add(skill);
            await _context.SaveChangesAsync();
            
            // Reload with navigation properties
            await _context.Entry(skill)
                .Reference(ups => ups.Skill)
                .LoadAsync();
            
            return skill;
        }

        public async Task<IEnumerable<UserProfileSkill>> AddSkillsAsync(IEnumerable<UserProfileSkill> skills)
        {
            // Per SRS 2.3.2.1: Add Bulk Skills
            var skillsList = skills.ToList();
            _context.UserProfileSkills.AddRange(skillsList);
            await _context.SaveChangesAsync();
            
            // Reload with navigation properties
            foreach (var skill in skillsList)
            {
                await _context.Entry(skill)
                    .Reference(ups => ups.Skill)
                    .LoadAsync();
            }
            
            return skillsList;
        }

        public async Task<UserProfileSkill> UpdateSkillAsync(UserProfileSkill skill)
        {
            // Per SRS 2.3.2.3: Update Skills
            _context.UserProfileSkills.Update(skill);
            await _context.SaveChangesAsync();
            
            // Reload with navigation properties
            await _context.Entry(skill)
                .Reference(ups => ups.Skill)
                .LoadAsync();
            
            return skill;
        }

        public async Task<bool> DeleteSkillAsync(Guid id)
        {
            // Per SRS 2.3.2.4: Delete Skills
            var skill = await _context.UserProfileSkills.FindAsync(id);
            if (skill == null)
                return false;

            _context.UserProfileSkills.Remove(skill);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> DeleteSkillsAsync(IEnumerable<Guid> ids)
        {
            // Per SRS 2.3.2.4: Delete Bulk Skills
            var idsList = ids.ToList();
            var skills = await _context.UserProfileSkills
                .Where(ups => idsList.Contains(ups.Id))
                .ToListAsync();

            if (!skills.Any())
                return 0;

            _context.UserProfileSkills.RemoveRange(skills);
            await _context.SaveChangesAsync();
            return skills.Count;
        }

        // Analytics Methods per SRS 2.3.3

        public async Task<int> GetSkillsCountAsync(Guid userProfileId)
        {
            // Per SRS 2.3.1.3: Maximum 100 skills per profile
            return await _context.UserProfileSkills
                .CountAsync(ups => ups.UserProfileId == userProfileId);
        }

        public async Task<int> GetFeaturedCountAsync(Guid userProfileId)
        {
            // Per SRS 2.3.1.3: Maximum 10 featured skills
            return await _context.UserProfileSkills
                .CountAsync(ups => ups.UserProfileId == userProfileId && ups.IsFeatured);
        }

        public async Task<Dictionary<int, int>> GetSkillsByProficiencyAsync(Guid userProfileId)
        {
            // Per SRS 2.3.3.2: Skills Distribution by Proficiency
            return await _context.UserProfileSkills
                .Where(ups => ups.UserProfileId == userProfileId)
                .GroupBy(ups => ups.ProficiencyLevel)
                .Select(g => new { Level = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Level, x => x.Count);
        }

        public async Task<Dictionary<string, int>> GetSkillsByCategoryAsync(Guid userProfileId)
        {
            // Per SRS 2.3.3.1: Category Breakdown
            return await _context.UserProfileSkills
                .Include(ups => ups.Skill)
                .Where(ups => ups.UserProfileId == userProfileId)
                .GroupBy(ups => ups.Skill.Category.ToString())
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Category, x => x.Count);
        }

        public async Task<IEnumerable<UserProfileSkill>> GetSkillsTimelineAsync(Guid userProfileId)
        {
            // Per SRS 2.3.3.3: Skills Timeline
            return await _context.UserProfileSkills
                .Include(ups => ups.Skill)
                .Where(ups => ups.UserProfileId == userProfileId && ups.StartedDate.HasValue)
                .OrderBy(ups => ups.StartedDate)
                .ToListAsync();
        }

        public async Task<int> GetTotalExperienceYearsAsync(Guid userProfileId)
        {
            // Per SRS 2.3.3.1: Total Experience Years
            var total = await _context.UserProfileSkills
                .Where(ups => ups.UserProfileId == userProfileId && ups.YearsOfExperience.HasValue)
                .SumAsync(ups => ups.YearsOfExperience!.Value);
            
            return total;
        }
    }
}
