using Mentora.Domain.Common;
using Mentora.Domain.Entities;
using Mentora.Domain.Entities.Auth;
using Mentora.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Core Domain Entities
        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<UserStats> UserStats { get; set; }
        public DbSet<UserSkill> UserSkills { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<UserDiagnosticResponse> UserDiagnosticResponses { get; set; }

        // Career Builder Module
        public DbSet<CareerPlan> CareerPlans { get; set; }

        public DbSet<CareerStep> CareerSteps { get; set; }
        public DbSet<CareerPlanSkill> CareerPlanSkills { get; set; }
        public DbSet<StepCheckpoint> StepCheckpoints { get; set; }
        public DbSet<LearningResource> LearningResources { get; set; }

        // Scheduler Module
        public DbSet<StudyPlan> StudyPlans { get; set; }

        public DbSet<AvailabilitySlot> AvailabilitySlots { get; set; }
        public DbSet<StudyTask> StudyTasks { get; set; }
        public DbSet<StudySession> StudySessions { get; set; }
        public DbSet<TaskFeedbackLog> TaskFeedbackLogs { get; set; }

        // Engagement & AI Logging
        public DbSet<Achievement> Achievements { get; set; }

        public DbSet<UserAchievement> UserAchievements { get; set; }
        public DbSet<DailyReflection> DailyReflections { get; set; }
        public DbSet<AiRequestLog> AiRequestLogs { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<StudyTask>().Property(t => t.Status).HasConversion<string>();
            builder.Entity<StudyTask>().Property(t => t.Priority).HasConversion<string>();
            builder.Entity<CareerStep>().Property(s => s.Status).HasConversion<string>();
            builder.Entity<UserSkill>().Property(s => s.CurrentLevel).HasConversion<string>();
            builder.Entity<CareerPlanSkill>().Property(s => s.TargetLevel).HasConversion<string>();

            builder.Entity<User>()
                .HasOne<UserProfile>()
                .WithOne(p => p.User)
                .HasForeignKey<UserProfile>(p => p.UserId);

            builder.Entity<User>()
                .HasOne<UserStats>()
                .WithOne(s => s.User)
                .HasForeignKey<UserStats>(s => s.UserId);

            builder.Entity<StudyTask>()
                .HasOne(t => t.FeedbackLog)
                .WithOne(f => f.StudyTask)
                .HasForeignKey<TaskFeedbackLog>(f => f.StudyTaskId);

            builder.Entity<StudyTask>()
                .HasOne(t => t.CareerStep)
                .WithMany(s => s.LinkedStudyTasks)
                .HasForeignKey(t => t.CareerStepId)
                .OnDelete(DeleteBehavior.SetNull);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
                    var propertyMethodInfo = typeof(EF).GetMethod("Property")?.MakeGenericMethod(typeof(DateTime?));
                    var deletedAtProperty = System.Linq.Expressions.Expression.Call(propertyMethodInfo!, parameter, System.Linq.Expressions.Expression.Constant("DeletedAt"));
                    var compareExpression = System.Linq.Expressions.Expression.Equal(deletedAtProperty, System.Linq.Expressions.Expression.Constant(null));
                    var lambda = System.Linq.Expressions.Expression.Lambda(compareExpression, parameter);

                    builder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.DeletedAt = DateTime.UtcNow;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}