using Mentora.Application.DTOs.UserProfile;
using Mentora.Application.Interfaces.Repositories;
using Mentora.Application.Interfaces.Services;
using Mentora.Domain.Entities;

namespace Mentora.Infrastructure.Services
{
    /// <summary>
    /// Service implementation for UserProfileSkill operations
    /// Per SRS 2.3: Skills Portfolio Management
    /// </summary>
    public class UserProfileSkillService : IUserProfileSkillService
    {
        private readonly IUserProfileSkillRepository _skillRepository;
        private readonly ISkillRepository _masterSkillRepository;
        private readonly IUserProfileRepository _profileRepository;

        public UserProfileSkillService(
            IUserProfileSkillRepository skillRepository,
            ISkillRepository masterSkillRepository,
            IUserProfileRepository profileRepository)
        {
            _skillRepository = skillRepository;
            _masterSkillRepository = masterSkillRepository;
            _profileRepository = profileRepository;
        }

        public async Task<IEnumerable<UserProfileSkillDto>> GetUserSkillsAsync(
            Guid userId,
            int? proficiencyLevel = null,
            bool? isFeatured = null,
            string? sortBy = null)
        {
            var profile = await GetUserProfileAsync(userId);
            var skills = await _skillRepository.GetUserSkillsAsync(
                profile.Id, proficiencyLevel, isFeatured, sortBy);

            return skills.Select(MapToDto);
        }

        public async Task<UserProfileSkillDto?> GetSkillByIdAsync(Guid userId, Guid skillId)
        {
            var profile = await GetUserProfileAsync(userId);
            var skill = await _skillRepository.GetByIdAsync(skillId);

            if (skill == null || skill.UserProfileId != profile.Id)
                return null;

            return MapToDto(skill);
        }

        public async Task<IEnumerable<UserProfileSkillDto>> GetFeaturedSkillsAsync(Guid userId)
        {
            var profile = await GetUserProfileAsync(userId);
            var skills = await _skillRepository.GetFeaturedSkillsAsync(profile.Id);

            return skills.Select(MapToDto);
        }

        public async Task<UserProfileSkillDto> AddSkillAsync(Guid userId, AddSkillDto dto)
        {
            var profile = await GetUserProfileAsync(userId);

            // Business Rule: Maximum 100 skills per profile (SRS 2.3.1.3)
            var skillsCount = await _skillRepository.GetSkillsCountAsync(profile.Id);
            if (skillsCount >= 100)
                throw new InvalidOperationException("Maximum 100 skills per profile");

            // Business Rule: Duplicate prevention (SRS 2.3.8.4)
            var exists = await _skillRepository.SkillExistsAsync(profile.Id, dto.SkillId);
            if (exists)
                throw new InvalidOperationException("Skill already added to profile");

            // Business Rule: Maximum 10 featured skills (SRS 2.3.1.3)
            if (dto.IsFeatured)
            {
                var featuredCount = await _skillRepository.GetFeaturedCountAsync(profile.Id);
                if (featuredCount >= 10)
                    throw new InvalidOperationException("Maximum 10 featured skills");
            }

            // Validate skill exists (SRS 2.3.8.3)
            var masterSkill = await _masterSkillRepository.GetByIdAsync(dto.SkillId);
            if (masterSkill == null)
                throw new KeyNotFoundException("Skill not found");

            var skill = new UserProfileSkill
            {
                UserProfileId = profile.Id,
                SkillId = dto.SkillId,
                ProficiencyLevel = dto.ProficiencyLevel,
                AcquisitionMethod = dto.AcquisitionMethod,
                StartedDate = dto.StartedDate,
                YearsOfExperience = dto.YearsOfExperience,
                IsFeatured = dto.IsFeatured,
                Notes = dto.Notes,
                DisplayOrder = dto.DisplayOrder
            };

            var created = await _skillRepository.AddSkillAsync(skill);
            return MapToDto(created);
        }

        public async Task<BulkOperationResultDto> AddSkillsAsync(Guid userId, AddBulkSkillsDto dto)
        {
            var profile = await GetUserProfileAsync(userId);
            var result = new BulkOperationResultDto { Success = true };

            // Business Rule: Maximum 100 skills per profile
            var currentCount = await _skillRepository.GetSkillsCountAsync(profile.Id);
            if (currentCount + dto.Skills.Count > 100)
                throw new InvalidOperationException($"Cannot add {dto.Skills.Count} skills. Maximum 100 skills per profile");

            foreach (var skillDto in dto.Skills)
            {
                try
                {
                    await AddSkillAsync(userId, skillDto);
                    result.SuccessCount++;
                    result.Results.Add(new BulkOperationItemDto
                    {
                        ItemId = skillDto.SkillId,
                        Status = "success"
                    });
                }
                catch (Exception ex)
                {
                    result.FailedCount++;
                    result.Results.Add(new BulkOperationItemDto
                    {
                        ItemId = skillDto.SkillId,
                        Status = "failed",
                        Reason = ex.Message
                    });
                }
            }

            result.Success = result.FailedCount == 0;
            return result;
        }

        public async Task<UserProfileSkillDto> UpdateSkillAsync(Guid userId, Guid skillId, UpdateSkillDto dto)
        {
            var profile = await GetUserProfileAsync(userId);
            var skill = await _skillRepository.GetByIdAsync(skillId);

            if (skill == null || skill.UserProfileId != profile.Id)
                throw new KeyNotFoundException("Skill not found");

            // Business Rule: Maximum 10 featured skills
            if (dto.IsFeatured == true && !skill.IsFeatured)
            {
                var featuredCount = await _skillRepository.GetFeaturedCountAsync(profile.Id);
                if (featuredCount >= 10)
                    throw new InvalidOperationException("Maximum 10 featured skills");
            }

            // Update only provided fields (SRS 2.3.2.3: Partial updates)
            if (dto.ProficiencyLevel.HasValue)
                skill.ProficiencyLevel = dto.ProficiencyLevel.Value;

            if (dto.AcquisitionMethod != null)
                skill.AcquisitionMethod = dto.AcquisitionMethod;

            if (dto.StartedDate.HasValue)
                skill.StartedDate = dto.StartedDate;

            if (dto.YearsOfExperience.HasValue)
                skill.YearsOfExperience = dto.YearsOfExperience;

            if (dto.IsFeatured.HasValue)
                skill.IsFeatured = dto.IsFeatured.Value;

            if (dto.Notes != null)
                skill.Notes = dto.Notes;

            if (dto.DisplayOrder.HasValue)
                skill.DisplayOrder = dto.DisplayOrder.Value;

            var updated = await _skillRepository.UpdateSkillAsync(skill);
            return MapToDto(updated);
        }

        public async Task<bool> DeleteSkillAsync(Guid userId, Guid skillId)
        {
            var profile = await GetUserProfileAsync(userId);
            var skill = await _skillRepository.GetByIdAsync(skillId);

            if (skill == null || skill.UserProfileId != profile.Id)
                return false;

            return await _skillRepository.DeleteSkillAsync(skillId);
        }

        public async Task<BulkOperationResultDto> DeleteSkillsAsync(Guid userId, DeleteBulkSkillsDto dto)
        {
            var profile = await GetUserProfileAsync(userId);
            var result = new BulkOperationResultDto { Success = true };

            foreach (var skillId in dto.SkillIds)
            {
                try
                {
                    var deleted = await DeleteSkillAsync(userId, skillId);
                    if (deleted)
                    {
                        result.SuccessCount++;
                        result.Results.Add(new BulkOperationItemDto
                        {
                            ItemId = skillId,
                            Status = "success"
                        });
                    }
                    else
                    {
                        result.FailedCount++;
                        result.Results.Add(new BulkOperationItemDto
                        {
                            ItemId = skillId,
                            Status = "failed",
                            Reason = "Skill not found"
                        });
                    }
                }
                catch (Exception ex)
                {
                    result.FailedCount++;
                    result.Results.Add(new BulkOperationItemDto
                    {
                        ItemId = skillId,
                        Status = "failed",
                        Reason = ex.Message
                    });
                }
            }

            result.Success = result.FailedCount == 0;
            return result;
        }

        public async Task<bool> ToggleFeaturedAsync(Guid userId, Guid skillId)
        {
            var profile = await GetUserProfileAsync(userId);
            var skill = await _skillRepository.GetByIdAsync(skillId);

            if (skill == null || skill.UserProfileId != profile.Id)
                return false;

            // If toggling to featured, check limit
            if (!skill.IsFeatured)
            {
                var featuredCount = await _skillRepository.GetFeaturedCountAsync(profile.Id);
                if (featuredCount >= 10)
                    throw new InvalidOperationException("Maximum 10 featured skills");
            }

            skill.IsFeatured = !skill.IsFeatured;
            await _skillRepository.UpdateSkillAsync(skill);
            return true;
        }

        public async Task<bool> ReorderSkillsAsync(Guid userId, ReorderSkillsDto dto)
        {
            var profile = await GetUserProfileAsync(userId);

            foreach (var order in dto.SkillOrders)
            {
                var skill = await _skillRepository.GetByIdAsync(order.SkillId);
                if (skill != null && skill.UserProfileId == profile.Id)
                {
                    skill.DisplayOrder = order.DisplayOrder;
                    await _skillRepository.UpdateSkillAsync(skill);
                }
            }

            return true;
        }

        // Analytics Methods per SRS 2.3.3

        public async Task<SkillsSummaryDto> GetSkillsSummaryAsync(Guid userId)
        {
            var profile = await GetUserProfileAsync(userId);

            var totalSkills = await _skillRepository.GetSkillsCountAsync(profile.Id);
            var proficiencyDist = await _skillRepository.GetSkillsByProficiencyAsync(profile.Id);
            var categoryBreakdown = await _skillRepository.GetSkillsByCategoryAsync(profile.Id);
            var totalExperience = await _skillRepository.GetTotalExperienceYearsAsync(profile.Id);
            var featuredCount = await _skillRepository.GetFeaturedCountAsync(profile.Id);

            return new SkillsSummaryDto
            {
                TotalSkills = totalSkills,
                ProficiencyDistribution = proficiencyDist.ToDictionary(
                    x => GetProficiencyName(x.Key),
                    x => x.Value
                ),
                CategoryBreakdown = categoryBreakdown,
                TotalExperienceYears = totalExperience,
                FeaturedSkillsCount = featuredCount
            };
        }

        public async Task<SkillsDistributionDto> GetSkillsDistributionAsync(Guid userId)
        {
            var profile = await GetUserProfileAsync(userId);

            var totalSkills = await _skillRepository.GetSkillsCountAsync(profile.Id);
            if (totalSkills == 0)
                return new SkillsDistributionDto();

            var proficiencyDist = await _skillRepository.GetSkillsByProficiencyAsync(profile.Id);
            var categoryDist = await _skillRepository.GetSkillsByCategoryAsync(profile.Id);

            return new SkillsDistributionDto
            {
                ByProficiency = proficiencyDist.ToDictionary(
                    x => GetProficiencyName(x.Key),
                    x => Math.Round((double)x.Value / totalSkills * 100, 2)
                ),
                ByCategory = categoryDist.ToDictionary(
                    x => x.Key,
                    x => Math.Round((double)x.Value / totalSkills * 100, 2)
                )
            };
        }

        public async Task<SkillsTimelineDto> GetSkillsTimelineAsync(Guid userId)
        {
            var profile = await GetUserProfileAsync(userId);
            var skills = await _skillRepository.GetSkillsTimelineAsync(profile.Id);

            var timeline = skills
                .GroupBy(s => s.StartedDate!.Value.Year)
                .Select(yearGroup => new TimelineYearDto
                {
                    Year = yearGroup.Key,
                    Months = yearGroup
                        .GroupBy(s => s.StartedDate!.Value.Month)
                        .Select(monthGroup => new TimelineMonthDto
                        {
                            Month = monthGroup.Key,
                            Skills = monthGroup.Select(s => new TimelineSkillDto
                            {
                                Name = s.Skill.Name,
                                Proficiency = GetProficiencyName(s.ProficiencyLevel)
                            }).ToList()
                        }).ToList()
                }).ToList();

            return new SkillsTimelineDto { Timeline = timeline };
        }

        public async Task<SkillsCoverageDto> GetSkillsCoverageAsync(Guid userId)
        {
            var profile = await GetUserProfileAsync(userId);
            
            var totalSkills = await _skillRepository.GetSkillsCountAsync(profile.Id);
            var categoryBreakdown = await _skillRepository.GetSkillsByCategoryAsync(profile.Id);

            // Calculate coverage score (0-100)
            var coverageScore = Math.Min(totalSkills * 10, 100); // Simple: 10 points per skill, max 100

            // Identify strong and weak categories
            var strongCategories = categoryBreakdown
                .Where(c => c.Value >= 5)
                .Select(c => c.Key)
                .ToList();

            var weakCategories = categoryBreakdown
                .Where(c => c.Value < 3)
                .Select(c => c.Key)
                .ToList();

            // Generate recommendations
            var recommendations = new List<string>();
            if (totalSkills < 10)
                recommendations.Add("Add more skills to improve profile visibility");
            if (weakCategories.Any())
                recommendations.Add($"Consider adding skills in: {string.Join(", ", weakCategories)}");
            if (strongCategories.Count == 1)
                recommendations.Add("Diversify your skills across multiple categories");

            return new SkillsCoverageDto
            {
                CoverageScore = coverageScore,
                StrongCategories = strongCategories,
                WeakCategories = weakCategories,
                Recommendations = recommendations,
                GapAnalysis = categoryBreakdown.ToDictionary(
                    c => c.Key,
                    c => new CategoryCoverageDto
                    {
                        Coverage = Math.Min(c.Value * 20, 100),
                        Missing = new List<string>()
                    }
                )
            };
        }

        // Helper Methods

        private async Task<UserProfile> GetUserProfileAsync(Guid userId)
        {
            var profile = await _profileRepository.GetByUserIdAsync(userId);
            if (profile == null)
                throw new KeyNotFoundException("User profile not found");
            return profile;
        }

        private UserProfileSkillDto MapToDto(UserProfileSkill skill)
        {
            return new UserProfileSkillDto
            {
                Id = skill.Id,
                UserProfileId = skill.UserProfileId,
                SkillId = skill.SkillId,
                SkillName = skill.Skill?.Name ?? string.Empty,
                SkillCategory = skill.Skill?.Category.ToString() ?? string.Empty,
                ProficiencyLevel = skill.ProficiencyLevel,
                ProficiencyLevelName = GetProficiencyName(skill.ProficiencyLevel),
                AcquisitionMethod = skill.AcquisitionMethod,
                StartedDate = skill.StartedDate,
                YearsOfExperience = skill.YearsOfExperience,
                IsFeatured = skill.IsFeatured,
                Notes = skill.Notes,
                DisplayOrder = skill.DisplayOrder,
                CreatedAt = skill.CreatedAt,
                UpdatedAt = skill.UpdatedAt
            };
        }

        private string GetProficiencyName(int level)
        {
            return level switch
            {
                0 => "Beginner",
                1 => "Intermediate",
                2 => "Advanced",
                3 => "Expert",
                _ => "Unknown"
            };
        }
    }
}
