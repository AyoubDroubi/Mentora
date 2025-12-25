using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentora.Domain.Entities
{
    public class UserAchievement
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [ForeignKey("Achievement")]
        public int AchievementId { get; set; }
        public Achievement Achievement { get; set; } = null!;

        public DateTime EarnedAt { get; set; } = DateTime.UtcNow;
    }
}