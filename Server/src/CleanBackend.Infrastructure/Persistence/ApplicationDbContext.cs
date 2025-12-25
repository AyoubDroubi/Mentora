using CleanBackend.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text.Json; 

namespace CleanBackend.Infrastructure.Persistence
{

    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CareerPlan> CareerPlans { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

           
            builder.Entity<CareerPlan>(entity =>
            {
               
                entity.Property(e => e.Steps)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                          v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>()
                      );
            });

            
            builder.Entity<CareerPlan>().ToTable("CareerPlans");
            builder.Entity<PasswordResetToken>().ToTable("PasswordResetTokens");
        }
    }
}