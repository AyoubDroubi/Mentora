using Mentora.Domain.Common;
using Mentora.Domain.Entities;
using Mentora.Domain.Entities.Assessment;
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
        public DbSet<UserSkill> UserSkills { get; set; } // Legacy - will be removed
        public DbSet<UserProfileSkill> UserProfileSkills { get; set; } // New per SRS 2.3.1
        public DbSet<Skill> Skills { get; set; }
        public DbSet<UserDiagnosticResponse> UserDiagnosticResponses { get; set; }

        // Career Builder Module per SRS Feature 1-4
        public DbSet<CareerPlan> CareerPlans { get; set; }
        public DbSet<CareerStep> CareerSteps { get; set; }
        public DbSet<CareerPlanSkill> CareerPlanSkills { get; set; }
        public DbSet<CareerQuizAttempt> CareerQuizAttempts { get; set; }
        public DbSet<StepCheckpoint> StepCheckpoints { get; set; }
        public DbSet<LearningResource> LearningResources { get; set; }

        // AI Assessment & Study Plan Module per SRS Section 3 & 4
        public DbSet<AssessmentQuestion> AssessmentQuestions { get; set; }
        public DbSet<AssessmentResponse> AssessmentResponses { get; set; }
        public DbSet<AssessmentAttempt> AssessmentAttempts { get; set; }
        public DbSet<AiStudyPlan> AiStudyPlans { get; set; }
        public DbSet<StudyPlanStep> StudyPlanSteps { get; set; }
        public DbSet<StudyPlanCheckpoint> StudyPlanCheckpoints { get; set; }
        public DbSet<StudyPlanResource> StudyPlanResources { get; set; }
        public DbSet<StudyPlanSkill> StudyPlanSkills { get; set; }

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
            builder.Entity<UserSkill>().Property(s => s.CurrentLevel).HasConversion<string>(); // Legacy
            
            // Career Builder enums per SRS
            builder.Entity<CareerPlan>().Property(p => p.Status).HasConversion<string>();
            builder.Entity<CareerPlanSkill>().Property(s => s.Status).HasConversion<string>();
            builder.Entity<CareerPlanSkill>().Property(s => s.TargetLevel).HasConversion<string>();
            builder.Entity<Skill>().Property(s => s.Category).HasConversion<string>();
            builder.Entity<CareerQuizAttempt>().Property(q => q.Status).HasConversion<string>();

            // Assessment Module enums per SRS Section 3
            builder.Entity<AssessmentQuestion>().Property(q => q.QuestionType).HasConversion<string>();
            builder.Entity<AssessmentAttempt>().Property(a => a.Status).HasConversion<string>();
            builder.Entity<AssessmentAttempt>().Property(a => a.StudyLevel).HasConversion<string>();
            builder.Entity<AiStudyPlan>().Property(p => p.DifficultyLevel).HasConversion<string>();
            builder.Entity<AiStudyPlan>().Property(p => p.Status).HasConversion<string>();
            builder.Entity<StudyPlanStep>().Property(s => s.Status).HasConversion<string>();
            builder.Entity<StudyPlanResource>().Property(r => r.ResourceType).HasConversion<string>();
            builder.Entity<StudyPlanSkill>().Property(s => s.TargetProficiency).HasConversion<string>();
            builder.Entity<StudyPlanSkill>().Property(s => s.Status).HasConversion<string>();

            // UserProfileSkill Configuration per SRS 2.3.1
            ConfigureUserProfileSkills(builder);

            // Assessment Module Configuration per SRS Section 3 & 4
            ConfigureAssessmentModule(builder);

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

            // StudyTask -> StudyPlanStep (Many-to-One) per SRS 5.1
            builder.Entity<StudyTask>()
                .HasOne(t => t.StudyPlanStep)
                .WithMany(s => s.Tasks)
                .HasForeignKey(t => t.StudyPlanStepId)
                .OnDelete(DeleteBehavior.SetNull);

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

        /// <summary>
        /// Configure UserProfileSkill entity per SRS 2.3.1
        /// Includes unique constraints, indexes, and relationships
        /// </summary>
        private void ConfigureUserProfileSkills(ModelBuilder builder)
        {
            // Unique Constraint per SRS 2.3.1.2
            builder.Entity<UserProfileSkill>()
                .HasIndex(ups => new { ups.UserProfileId, ups.SkillId })
                .IsUnique()
                .HasDatabaseName("IX_UserProfileSkills_UserProfile_Skill_Unique");

            // Performance Indexes per SRS 2.3.9.2
            builder.Entity<UserProfileSkill>()
                .HasIndex(ups => ups.UserProfileId)
                .HasDatabaseName("IX_UserProfileSkills_UserProfileId");

            builder.Entity<UserProfileSkill>()
                .HasIndex(ups => ups.SkillId)
                .HasDatabaseName("IX_UserProfileSkills_SkillId");

            builder.Entity<UserProfileSkill>()
                .HasIndex(ups => ups.IsFeatured)
                .HasDatabaseName("IX_UserProfileSkills_IsFeatured");

            builder.Entity<UserProfileSkill>()
                .HasIndex(ups => ups.DisplayOrder)
                .HasDatabaseName("IX_UserProfileSkills_DisplayOrder");

            builder.Entity<UserProfileSkill>()
                .HasIndex(ups => ups.ProficiencyLevel)
                .HasDatabaseName("IX_UserProfileSkills_ProficiencyLevel");

            // Relationships per SRS 2.3.1
            
            // UserProfile -> UserProfileSkills (One-to-Many)
            builder.Entity<UserProfile>()
                .HasMany(up => up.Skills)
                .WithOne(ups => ups.UserProfile)
                .HasForeignKey(ups => ups.UserProfileId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete per SRS 8.3

            // Skill -> UserProfileSkills (One-to-Many)
            builder.Entity<Skill>()
                .HasMany(s => s.UserProfileSkills)
                .WithOne(ups => ups.Skill)
                .HasForeignKey(ups => ups.SkillId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict deletion per SRS 8.3
        }

        /// <summary>
        /// Configure Assessment Module entities per SRS Section 3 & 4
        /// Includes relationships, constraints, and indexes per SRS 8.4 & 8.5
        /// </summary>
        private void ConfigureAssessmentModule(ModelBuilder builder)
        {
            // AssessmentQuestion Indexes per SRS 8.5
            builder.Entity<AssessmentQuestion>()
                .HasIndex(q => q.TargetMajor)
                .HasDatabaseName("IX_AssessmentQuestions_TargetMajor");

            builder.Entity<AssessmentQuestion>()
                .HasIndex(q => new { q.IsActive, q.OrderIndex })
                .HasDatabaseName("IX_AssessmentQuestions_Active_Order");

            // AssessmentResponse Unique Constraint per SRS 8.4
            builder.Entity<AssessmentResponse>()
                .HasIndex(r => new { r.AssessmentAttemptId, r.QuestionId })
                .IsUnique()
                .HasDatabaseName("IX_AssessmentResponses_Attempt_Question_Unique");

            // AssessmentResponse Indexes per SRS 8.5
            builder.Entity<AssessmentResponse>()
                .HasIndex(r => r.UserId)
                .HasDatabaseName("IX_AssessmentResponses_UserId");

            // AssessmentAttempt Indexes per SRS 8.5
            builder.Entity<AssessmentAttempt>()
                .HasIndex(a => new { a.UserId, a.Status })
                .HasDatabaseName("IX_AssessmentAttempts_User_Status");

            // AiStudyPlan Indexes per SRS 8.5
            builder.Entity<AiStudyPlan>()
                .HasIndex(p => new { p.UserId, p.IsActive, p.Status })
                .HasDatabaseName("IX_AiStudyPlans_User_Active_Status");

            builder.Entity<AiStudyPlan>()
                .HasIndex(p => p.AssessmentAttemptId)
                .HasDatabaseName("IX_AiStudyPlans_AssessmentAttemptId");

            builder.Entity<AiStudyPlan>()
                .HasIndex(p => p.CareerPlanId)
                .HasDatabaseName("IX_AiStudyPlans_CareerPlanId");

            // StudyPlanStep Indexes per SRS 8.5
            builder.Entity<StudyPlanStep>()
                .HasIndex(s => new { s.StudyPlanId, s.OrderIndex })
                .HasDatabaseName("IX_StudyPlanSteps_Plan_Order");

            // StudyPlanSkill Unique Constraint per SRS 8.4
            builder.Entity<StudyPlanSkill>()
                .HasIndex(s => new { s.StudyPlanId, s.SkillId })
                .IsUnique()
                .HasDatabaseName("IX_StudyPlanSkills_Plan_Skill_Unique");

            // StudyPlanSkill Indexes per SRS 8.5
            builder.Entity<StudyPlanSkill>()
                .HasIndex(s => s.Status)
                .HasDatabaseName("IX_StudyPlanSkills_Status");

            // Relationships per SRS Section 3 & 4

            // User -> AssessmentAttempts (One-to-Many)
            builder.Entity<User>()
                .HasMany<AssessmentAttempt>()
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> AssessmentResponses (One-to-Many)
            builder.Entity<User>()
                .HasMany<AssessmentResponse>()
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> AiStudyPlans (One-to-Many) - NoAction to avoid cascade cycles per SRS 8.3
            builder.Entity<User>()
                .HasMany<AiStudyPlan>()
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction); // Changed from Cascade to avoid multiple cascade paths

            // AssessmentAttempt -> AssessmentResponses (One-to-Many)
            builder.Entity<AssessmentAttempt>()
                .HasMany(a => a.Responses)
                .WithOne(r => r.AssessmentAttempt)
                .HasForeignKey(r => r.AssessmentAttemptId)
                .OnDelete(DeleteBehavior.Cascade);

            // AssessmentAttempt -> AiStudyPlans (One-to-Many)
            builder.Entity<AssessmentAttempt>()
                .HasMany(a => a.GeneratedStudyPlans)
                .WithOne(p => p.AssessmentAttempt)
                .HasForeignKey(p => p.AssessmentAttemptId)
                .OnDelete(DeleteBehavior.SetNull);

            // AssessmentQuestion -> AssessmentResponses (One-to-Many)
            builder.Entity<AssessmentQuestion>()
                .HasMany(q => q.Responses)
                .WithOne(r => r.Question)
                .HasForeignKey(r => r.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            // CareerPlan -> AiStudyPlans (One-to-Many) per SRS 5.1
            builder.Entity<CareerPlan>()
                .HasMany<AiStudyPlan>()
                .WithOne(p => p.CareerPlan)
                .HasForeignKey(p => p.CareerPlanId)
                .OnDelete(DeleteBehavior.SetNull);

            // AiStudyPlan -> StudyPlanSteps (One-to-Many) per SRS 3.2.2
            builder.Entity<AiStudyPlan>()
                .HasMany(p => p.Steps)
                .WithOne(s => s.StudyPlan)
                .HasForeignKey(s => s.StudyPlanId)
                .OnDelete(DeleteBehavior.Cascade);

            // AiStudyPlan -> StudyPlanResources (One-to-Many) per SRS 3.2.3
            builder.Entity<AiStudyPlan>()
                .HasMany(p => p.Resources)
                .WithOne(r => r.StudyPlan)
                .HasForeignKey(r => r.StudyPlanId)
                .OnDelete(DeleteBehavior.Cascade);

            // AiStudyPlan -> StudyPlanSkills (One-to-Many) per SRS 3.3
            builder.Entity<AiStudyPlan>()
                .HasMany(p => p.RequiredSkills)
                .WithOne(s => s.StudyPlan)
                .HasForeignKey(s => s.StudyPlanId)
                .OnDelete(DeleteBehavior.Cascade);

            // StudyPlanStep -> StudyPlanCheckpoints (One-to-Many) per SRS 3.2.2
            builder.Entity<StudyPlanStep>()
                .HasMany(s => s.Checkpoints)
                .WithOne(c => c.StudyPlanStep)
                .HasForeignKey(c => c.StudyPlanStepId)
                .OnDelete(DeleteBehavior.Cascade);

            // StudyPlanStep -> StudyPlanResources (One-to-Many)
            builder.Entity<StudyPlanStep>()
                .HasMany<StudyPlanResource>()
                .WithOne(r => r.StudyPlanStep)
                .HasForeignKey(r => r.StudyPlanStepId)
                .OnDelete(DeleteBehavior.SetNull);

            // StudyPlanCheckpoint -> StudyTask (One-to-One) per SRS 5.1.2
            builder.Entity<StudyPlanCheckpoint>()
                .HasOne(c => c.StudyTask)
                .WithOne()
                .HasForeignKey<StudyPlanCheckpoint>(c => c.StudyTaskId)
                .OnDelete(DeleteBehavior.SetNull);

            // StudyPlanSkill -> Skill (Many-to-One) per SRS 3.3.2
            builder.Entity<StudyPlanSkill>()
                .HasOne(s => s.Skill)
                .WithMany()
                .HasForeignKey(s => s.SkillId)
                .OnDelete(DeleteBehavior.Restrict);

            // AiStudyPlan -> AiRequestLog (Many-to-One) per SRS 7.1
            builder.Entity<AiStudyPlan>()
                .HasOne(p => p.AiRequestLog)
                .WithMany()
                .HasForeignKey(p => p.AiRequestLogId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}