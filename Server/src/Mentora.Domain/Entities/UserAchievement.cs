using Mentora.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentora.Domain.Entities
{
    public class UserAchievement : BaseEntity
    {
        public Guid UserId { get; set; }
        public int AchievementId { get; set; }
        public Achievement Achievement { get; set; } = null!;
        public DateTime EarnedAt { get; set; } = DateTime.UtcNow;
    }
}