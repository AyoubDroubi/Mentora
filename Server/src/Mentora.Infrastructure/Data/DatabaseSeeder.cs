using Mentora.Domain.Entities;
using Mentora.Domain.Entities.Auth;
using Mentora.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Mentora.Infrastructure.Data
{
    /// <summary>
    /// Database seeder for initial development data
    /// Seeds two test users: Saad and Maria with complete profiles
    /// </summary>
    public static class DatabaseSeeder
    {
        /// <summary>
        /// Seeds the database with initial data
        /// </summary>
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

            try
            {
                // Ensure database is created
                await context.Database.MigrateAsync();

                // Check if data already exists
                if (await context.Users.AnyAsync())
                {
                    logger.LogInformation("Database already seeded. Skipping seed.");
                    return;
                }

                logger.LogInformation("Starting database seeding...");

                // Seed Skills first
                var skills = await SeedSkillsAsync(context, logger);

                // Seed Achievements
                var achievements = await SeedAchievementsAsync(context, logger);

                // Seed Users
                var saad = await SeedSaadAsync(userManager, context, skills, achievements, logger);
                var maria = await SeedMariaAsync(userManager, context, skills, achievements, logger);

                logger.LogInformation("Database seeding completed successfully!");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database");
                throw;
            }
        }

        private static async Task<List<Skill>> SeedSkillsAsync(ApplicationDbContext context, ILogger logger)
        {
            logger.LogInformation("Seeding Skills...");

            var skills = new List<Skill>
            {
                new Skill { Id = Guid.NewGuid(), Name = "C#" },
                new Skill { Id = Guid.NewGuid(), Name = "JavaScript" },
                new Skill { Id = Guid.NewGuid(), Name = "Python" },
                new Skill { Id = Guid.NewGuid(), Name = "SQL" },
                new Skill { Id = Guid.NewGuid(), Name = "React" },
                new Skill { Id = Guid.NewGuid(), Name = "ASP.NET Core" },
                new Skill { Id = Guid.NewGuid(), Name = "Node.js" },
                new Skill { Id = Guid.NewGuid(), Name = "Docker" },
                new Skill { Id = Guid.NewGuid(), Name = "Git" },
                new Skill { Id = Guid.NewGuid(), Name = "Azure" },
                new Skill { Id = Guid.NewGuid(), Name = "Machine Learning" },
                new Skill { Id = Guid.NewGuid(), Name = "Data Analysis" },
                new Skill { Id = Guid.NewGuid(), Name = "UI/UX Design" },
                new Skill { Id = Guid.NewGuid(), Name = "Project Management" },
                new Skill { Id = Guid.NewGuid(), Name = "Communication" }
            };

            await context.Skills.AddRangeAsync(skills);
            await context.SaveChangesAsync();

            logger.LogInformation($"Seeded {skills.Count} skills");
            return skills;
        }

        private static async Task<List<Achievement>> SeedAchievementsAsync(ApplicationDbContext context, ILogger logger)
        {
            logger.LogInformation("Seeding Achievements...");

            var achievements = new List<Achievement>
            {
                new Achievement
                {
                    Id = Guid.NewGuid(),
                    Name = "First Steps",
                    Description = "Complete your first study session",
                    IconKey = "first-steps",
                    XpReward = 50
                },
                new Achievement
                {
                    Id = Guid.NewGuid(),
                    Name = "Week Warrior",
                    Description = "Maintain a 7-day study streak",
                    IconKey = "week-warrior",
                    XpReward = 100
                },
                new Achievement
                {
                    Id = Guid.NewGuid(),
                    Name = "Task Master",
                    Description = "Complete 10 study tasks",
                    IconKey = "task-master",
                    XpReward = 150
                },
                new Achievement
                {
                    Id = Guid.NewGuid(),
                    Name = "Career Planner",
                    Description = "Create your first career plan",
                    IconKey = "career-planner",
                    XpReward = 200
                },
                new Achievement
                {
                    Id = Guid.NewGuid(),
                    Name = "Skill Collector",
                    Description = "Add 5 skills to your profile",
                    IconKey = "skill-collector",
                    XpReward = 75
                }
            };

            await context.Achievements.AddRangeAsync(achievements);
            await context.SaveChangesAsync();

            logger.LogInformation($"Seeded {achievements.Count} achievements");
            return achievements;
        }

        private static async Task<User> SeedSaadAsync(
            UserManager<User> userManager,
            ApplicationDbContext context,
            List<Skill> skills,
            List<Achievement> achievements,
            ILogger logger)
        {
            logger.LogInformation("Seeding user: Saad...");

            var saad = new User
            {
                Id = Guid.NewGuid(),
                UserName = "saad@mentora.com",
                Email = "saad@mentora.com",
                EmailConfirmed = true,
                FirstName = "???",
                LastName = "??????",
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddMonths(-2)
            };

            var result = await userManager.CreateAsync(saad, "Saad@123");
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to create user Saad: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            // User Profile
            var saadProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                UserId = saad.Id,
                Bio = "???? ???? ?????? ???? ?????? ????????? ??????? ?????????",
                Location = "?????? ??????",
                PhoneNumber = "+962791234567",
                DateOfBirth = new DateTime(2000, 5, 15),
                University = "??????? ????????",
                Major = "???? ???????",
                GraduationYear = 2024,
                LinkedInUrl = "https://linkedin.com/in/saad-ahmad",
                GitHubUrl = "https://github.com/saadahmad"
            };

            // User Stats
            var saadStats = new UserStats
            {
                Id = Guid.NewGuid(),
                UserId = saad.Id,
                TotalXP = 850,
                Level = 5,
                CurrentStreak = 12,
                TasksCompleted = 25
            };

            // User Skills
            var saadSkills = new List<UserSkill>
            {
                new UserSkill
                {
                    Id = Guid.NewGuid(),
                    UserId = saad.Id,
                    SkillId = skills.First(s => s.Name == "C#").Id,
                    CurrentLevel = SkillLevel.Intermediate
                },
                new UserSkill
                {
                    Id = Guid.NewGuid(),
                    UserId = saad.Id,
                    SkillId = skills.First(s => s.Name == "JavaScript").Id,
                    CurrentLevel = SkillLevel.Intermediate
                },
                new UserSkill
                {
                    Id = Guid.NewGuid(),
                    UserId = saad.Id,
                    SkillId = skills.First(s => s.Name == "SQL").Id,
                    CurrentLevel = SkillLevel.Beginner
                },
                new UserSkill
                {
                    Id = Guid.NewGuid(),
                    UserId = saad.Id,
                    SkillId = skills.First(s => s.Name == "React").Id,
                    CurrentLevel = SkillLevel.Beginner
                },
                new UserSkill
                {
                    Id = Guid.NewGuid(),
                    UserId = saad.Id,
                    SkillId = skills.First(s => s.Name == "Git").Id,
                    CurrentLevel = SkillLevel.Intermediate
                }
            };

            // User Achievements
            var saadAchievements = new List<UserAchievement>
            {
                new UserAchievement
                {
                    Id = Guid.NewGuid(),
                    UserId = saad.Id,
                    AchievementId = achievements.First(a => a.Name == "First Steps").Id,
                    EarnedAt = DateTime.UtcNow.AddMonths(-2)
                },
                new UserAchievement
                {
                    Id = Guid.NewGuid(),
                    UserId = saad.Id,
                    AchievementId = achievements.First(a => a.Name == "Week Warrior").Id,
                    EarnedAt = DateTime.UtcNow.AddMonths(-1)
                },
                new UserAchievement
                {
                    Id = Guid.NewGuid(),
                    UserId = saad.Id,
                    AchievementId = achievements.First(a => a.Name == "Task Master").Id,
                    EarnedAt = DateTime.UtcNow.AddDays(-15)
                }
            };

            // Career Plan
            var saadCareerPlan = new CareerPlan
            {
                Id = Guid.NewGuid(),
                UserId = saad.Id,
                Title = "Full Stack Developer",
                TargetRole = "Senior Full Stack Developer",
                Description = "??? ????? ???? Full Stack ????? ????? ?? .NET ? React",
                TimelineMonths = 18,
                CurrentStepIndex = 2,
                IsActive = true
            };

            // Career Steps
            var saadCareerSteps = new List<CareerStep>
            {
                new CareerStep
                {
                    Id = Guid.NewGuid(),
                    CareerPlanId = saadCareerPlan.Id,
                    Title = "????? ??????? ???????",
                    Description = "???? C# ? JavaScript ???? ?????",
                    OrderIndex = 1,
                    DurationWeeks = 12,
                    Status = CareerStepStatus.Completed
                },
                new CareerStep
                {
                    Id = Guid.NewGuid(),
                    CareerPlanId = saadCareerPlan.Id,
                    Title = "????? ?????? ?????",
                    Description = "???? 3 ?????? ????? ???????? ASP.NET Core ? React",
                    OrderIndex = 2,
                    DurationWeeks = 16,
                    Status = CareerStepStatus.InProgress
                },
                new CareerStep
                {
                    Id = Guid.NewGuid(),
                    CareerPlanId = saadCareerPlan.Id,
                    Title = "??????? ???? ?????",
                    Description = "???? Portfolio? ????? LinkedIn? ???????? ??? ?????",
                    OrderIndex = 3,
                    DurationWeeks = 8,
                    Status = CareerStepStatus.NotStarted
                }
            };

            await context.UserProfiles.AddAsync(saadProfile);
            await context.UserStats.AddAsync(saadStats);
            await context.UserSkills.AddRangeAsync(saadSkills);
            await context.UserAchievements.AddRangeAsync(saadAchievements);
            await context.CareerPlans.AddAsync(saadCareerPlan);
            await context.CareerSteps.AddRangeAsync(saadCareerSteps);
            await context.SaveChangesAsync();

            logger.LogInformation("User Saad seeded successfully with complete profile");
            return saad;
        }

        private static async Task<User> SeedMariaAsync(
            UserManager<User> userManager,
            ApplicationDbContext context,
            List<Skill> skills,
            List<Achievement> achievements,
            ILogger logger)
        {
            logger.LogInformation("Seeding user: Maria...");

            var maria = new User
            {
                Id = Guid.NewGuid(),
                UserName = "maria@mentora.com",
                Email = "maria@mentora.com",
                EmailConfirmed = true,
                FirstName = "?????",
                LastName = "????",
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddMonths(-3)
            };

            var result = await userManager.CreateAsync(maria, "Maria@123");
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to create user Maria: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            // User Profile
            var mariaProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                UserId = maria.Id,
                Bio = "????? ????? ???????? ????? ????? ???????? ??????? ?????????",
                Location = "???? ????????",
                PhoneNumber = "+971501234567",
                DateOfBirth = new DateTime(1999, 8, 20),
                University = "??????? ????????? ?? ???",
                Major = "????? ?????????",
                GraduationYear = 2023,
                LinkedInUrl = "https://linkedin.com/in/maria-haddad",
                GitHubUrl = "https://github.com/mariahaddad"
            };

            // User Stats
            var mariaStats = new UserStats
            {
                Id = Guid.NewGuid(),
                UserId = maria.Id,
                TotalXP = 1250,
                Level = 7,
                CurrentStreak = 21,
                TasksCompleted = 42
            };

            // User Skills
            var mariaSkills = new List<UserSkill>
            {
                new UserSkill
                {
                    Id = Guid.NewGuid(),
                    UserId = maria.Id,
                    SkillId = skills.First(s => s.Name == "Python").Id,
                    CurrentLevel = SkillLevel.Advanced
                },
                new UserSkill
                {
                    Id = Guid.NewGuid(),
                    UserId = maria.Id,
                    SkillId = skills.First(s => s.Name == "Machine Learning").Id,
                    CurrentLevel = SkillLevel.Intermediate
                },
                new UserSkill
                {
                    Id = Guid.NewGuid(),
                    UserId = maria.Id,
                    SkillId = skills.First(s => s.Name == "Data Analysis").Id,
                    CurrentLevel = SkillLevel.Intermediate
                },
                new UserSkill
                {
                    Id = Guid.NewGuid(),
                    UserId = maria.Id,
                    SkillId = skills.First(s => s.Name == "SQL").Id,
                    CurrentLevel = SkillLevel.Advanced
                },
                new UserSkill
                {
                    Id = Guid.NewGuid(),
                    UserId = maria.Id,
                    SkillId = skills.First(s => s.Name == "Git").Id,
                    CurrentLevel = SkillLevel.Advanced
                }
            };

            // User Achievements
            var mariaAchievements = new List<UserAchievement>
            {
                new UserAchievement
                {
                    Id = Guid.NewGuid(),
                    UserId = maria.Id,
                    AchievementId = achievements.First(a => a.Name == "First Steps").Id,
                    EarnedAt = DateTime.UtcNow.AddMonths(-3)
                },
                new UserAchievement
                {
                    Id = Guid.NewGuid(),
                    UserId = maria.Id,
                    AchievementId = achievements.First(a => a.Name == "Week Warrior").Id,
                    EarnedAt = DateTime.UtcNow.AddMonths(-2)
                },
                new UserAchievement
                {
                    Id = Guid.NewGuid(),
                    UserId = maria.Id,
                    AchievementId = achievements.First(a => a.Name == "Task Master").Id,
                    EarnedAt = DateTime.UtcNow.AddMonths(-1)
                },
                new UserAchievement
                {
                    Id = Guid.NewGuid(),
                    UserId = maria.Id,
                    AchievementId = achievements.First(a => a.Name == "Career Planner").Id,
                    EarnedAt = DateTime.UtcNow.AddMonths(-2)
                },
                new UserAchievement
                {
                    Id = Guid.NewGuid(),
                    UserId = maria.Id,
                    AchievementId = achievements.First(a => a.Name == "Skill Collector").Id,
                    EarnedAt = DateTime.UtcNow.AddMonths(-1)
                }
            };

            // Career Plan
            var mariaCareerPlan = new CareerPlan
            {
                Id = Guid.NewGuid(),
                UserId = maria.Id,
                Title = "Data Scientist",
                TargetRole = "Senior Data Scientist",
                Description = "??? ????? ????? ?????? ?????? ?????? ?? Machine Learning",
                TimelineMonths = 24,
                CurrentStepIndex = 3,
                IsActive = true
            };

            // Career Steps
            var mariaCareerSteps = new List<CareerStep>
            {
                new CareerStep
                {
                    Id = Guid.NewGuid(),
                    CareerPlanId = mariaCareerPlan.Id,
                    Title = "??????? ??? ????????",
                    Description = "????? Python? Pandas? NumPy? ?Matplotlib",
                    OrderIndex = 1,
                    DurationWeeks = 10,
                    Status = CareerStepStatus.Completed
                },
                new CareerStep
                {
                    Id = Guid.NewGuid(),
                    CareerPlanId = mariaCareerPlan.Id,
                    Title = "Machine Learning",
                    Description = "???? Scikit-learn? TensorFlow? ???????? ????????",
                    OrderIndex = 2,
                    DurationWeeks = 16,
                    Status = CareerStepStatus.Completed
                },
                new CareerStep
                {
                    Id = Guid.NewGuid(),
                    CareerPlanId = mariaCareerPlan.Id,
                    Title = "?????? ????? ?? Data Science",
                    Description = "???? 5 ?????? ????? ?????? ?ML",
                    OrderIndex = 3,
                    DurationWeeks = 20,
                    Status = CareerStepStatus.InProgress
                },
                new CareerStep
                {
                    Id = Guid.NewGuid(),
                    CareerPlanId = mariaCareerPlan.Id,
                    Title = "?????? ?????? ?? ???",
                    Description = "?????? ?? NLP ?? Computer Vision ???????? ??? ?????",
                    OrderIndex = 4,
                    DurationWeeks = 12,
                    Status = CareerStepStatus.NotStarted
                }
            };

            await context.UserProfiles.AddAsync(mariaProfile);
            await context.UserStats.AddAsync(mariaStats);
            await context.UserSkills.AddRangeAsync(mariaSkills);
            await context.UserAchievements.AddRangeAsync(mariaAchievements);
            await context.CareerPlans.AddAsync(mariaCareerPlan);
            await context.CareerSteps.AddRangeAsync(mariaCareerSteps);
            await context.SaveChangesAsync();

            logger.LogInformation("User Maria seeded successfully with complete profile");
            return maria;
        }
    }
}
