using Mentora.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Mentora.Domain.Entities.Auth
{
    /// <summary>
    /// User entity with GUID primary key per SRS 8.1
    /// Extended with security fields for comprehensive authentication
    /// </summary>
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public ICollection<CareerPlan> CareerPlans { get; set; } = new List<CareerPlan>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public UserProfile? UserProfile { get; set; }
    }
}