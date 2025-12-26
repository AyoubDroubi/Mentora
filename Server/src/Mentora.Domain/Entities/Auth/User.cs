using Mentora.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CleanBackend.Domain.Entities.Auth
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<CareerPlan> CareerPlans { get; set; }
    }
}