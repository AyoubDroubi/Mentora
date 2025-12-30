using Mentora.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Mentora.Domain.Entities.Auth
{
    // أضفنا <Guid> هنا لإجبار النظام على استخدام معرفات فريدة
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<CareerPlan> CareerPlans { get; set; } = new List<CareerPlan>();
    }
}