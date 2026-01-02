using Mentora.Domain.Common;
using Mentora.Domain.Entities;
using Mentora.Domain.Entities.Auth;
using Mentora.Domain.Entities.StudyPlanner;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Infrastructure.Persistence
{
    /// <summary>
    /// Application DbContext with Identity integration per SRS Module 1
    /// Supports GUID primary keys and soft delete per SRS 8.1 and 8.2
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
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

        // Career Builder Module per SRS Feature 1-4
        public DbSet<CareerPlan> CareerPlans { get; set; }
        public DbSet<CareerStep> CareerSteps { get; set; }
        public DbSet<CareerPlanSkill> CareerPlanSkills { get; set; }
        public DbSet<CareerQuizAttempt> CareerQuizAttempts { get; set; }
        public DbSet<StepCheckpoint> StepCheckpoints { get; set; }
        public DbSet<LearningResource> LearningResources { get; set; }

        // Scheduler Module
        public DbSet<StudyPlan> StudyPlans { get; set; }
        public DbSet<AvailabilitySlot> AvailabilitySlots { get; set; }
        public DbSet<StudyTask> StudyTasks { get; set; }
        public DbSet<StudySession> StudySessions { get; set; }
        public DbSet<TaskFeedbackLog> TaskFeedbackLogs { get; set; }

        // Study Planner Module
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<PlannerEvent> PlannerEvents { get; set; }
        public DbSet<UserNote> UserNotes { get; set; }
        public DbSet<StudyQuizAttempt> StudyQuizAttempts { get; set; }

        // Engagement & AI Logging
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }
        public DbSet<DailyReflection> DailyReflections { get; set; }
        public DbSet<AiRequestLog> AiRequestLogs { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        
        // Authentication Module per SRS 1.2
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Enums as strings for readability per SRS 8.1
            builder.Entity<StudyTask>().Property(t => t.Status).HasConversion<string>();
            builder.Entity<StudyTask>().Property(t => t.Priority).HasConversion<string>();
            builder.Entity<CareerStep>().Property(s => s.Status).HasConversion<string>();
            builder.Entity<UserSkill>().Property(s => s.CurrentLevel).HasConversion<string>();
            builder.Entity<CareerPlanSkill>().Property(s => s.TargetLevel).HasConversion<string>();
            
            // Career Builder enums per SRS
            builder.Entity<CareerPlan>().Property(p => p.Status).HasConversion<string>();
            builder.Entity<CareerPlanSkill>().Property(s => s.Status).HasConversion<string>();
            builder.Entity<Skill>().Property(s => s.Category).HasConversion<string>();
            builder.Entity<CareerQuizAttempt>().Property(q => q.Status).HasConversion<string>();

            // User Relationships
            
            // User -> CareerPlans (One-to-Many) per SRS Feature 1
            builder.Entity<User>()
                .HasMany(u => u.CareerPlans)
                .WithOne(cp => cp.User)
                .HasForeignKey(cp => cp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> RefreshTokens (One-to-Many) per SRS 1.2.2
            builder.Entity<User>()
                .HasMany(u => u.RefreshTokens)
                .WithOne(rt => rt.User)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> UserProfile (One-to-One) per SRS 2.1
            builder.Entity<User>()
                .HasOne(u => u.UserProfile)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> UserStats (One-to-One)
            builder.Entity<UserStats>()
                .HasOne(s => s.User)
                .WithOne()
                .HasForeignKey<UserStats>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Career Builder Relationships per SRS Feature 1-4
            
            // CareerPlan -> CareerQuizAttempt (Many-to-One) per SRS 6.4
            builder.Entity<CareerPlan>()
                .HasOne(p => p.CareerQuizAttempt)
                .WithMany(q => q.GeneratedPlans)
                .HasForeignKey(p => p.CareerQuizAttemptId)
                .OnDelete(DeleteBehavior.NoAction);
                
            // CareerPlan -> CareerSteps (One-to-Many) per SRS 3.3
            builder.Entity<CareerPlan>()
                .HasMany(p => p.Steps)
                .WithOne(s => s.CareerPlan)
                .HasForeignKey(s => s.CareerPlanId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // CareerPlan -> Skills (One-to-Many) per SRS 5.6
            builder.Entity<CareerPlan>()
                .HasMany(p => p.Skills)
                .WithOne(s => s.CareerPlan)
                .HasForeignKey(s => s.CareerPlanId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // CareerStep -> Skills (One-to-Many) per SRS 5.4
            builder.Entity<CareerStep>()
                .HasMany(s => s.Skills)
                .WithOne(ps => ps.CareerStep)
                .HasForeignKey(ps => ps.CareerStepId)
                .OnDelete(DeleteBehavior.NoAction);
                
            // CareerPlanSkill -> Skill (Many-to-One) per SRS 5.5
            builder.Entity<CareerPlanSkill>()
                .HasOne(ps => ps.Skill)
                .WithMany()
                .HasForeignKey(ps => ps.SkillId)
                .OnDelete(DeleteBehavior.Restrict);

            // Study Planner Relationships
            builder.Entity<TodoItem>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PlannerEvent>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserNote>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<StudyQuizAttempt>()
                .HasOne(q => q.User)
                .WithMany()
                .HasForeignKey(q => q.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // StudyTask -> FeedbackLog (One-to-One)
            builder.Entity<StudyTask>()
                .HasOne(t => t.FeedbackLog)
                .WithOne(f => f.StudyTask)
                .HasForeignKey<TaskFeedbackLog>(f => f.StudyTaskId)
                .OnDelete(DeleteBehavior.NoAction);

            // StudyTask -> CareerStep (Many-to-One) per SRS 5.1.1
            builder.Entity<StudyTask>()
                .HasOne(t => t.CareerStep)
                .WithMany(s => s.LinkedStudyTasks)
                .HasForeignKey(t => t.CareerStepId)
                .OnDelete(DeleteBehavior.NoAction);

            // Soft Delete Query Filter per SRS 8.2
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
                    var propertyMethodInfo = typeof(EF).GetMethod("Property")?.MakeGenericMethod(typeof(bool));
                    var isDeletedProperty = System.Linq.Expressions.Expression.Call(
                        propertyMethodInfo!, 
                        parameter, 
                        System.Linq.Expressions.Expression.Constant("IsDeleted")
                    );
                    var compareExpression = System.Linq.Expressions.Expression.Equal(
                        isDeletedProperty, 
                        System.Linq.Expressions.Expression.Constant(false)
                    );
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
                        entry.Entity.IsDeleted = true;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}