using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentora.Domain.Entities
{
    public class UserStats
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public int TotalXP { get; set; } = 0;
        public int Level { get; set; } = 1;

        public int CurrentStreak { get; set; } = 0; // كم يوم ورا بعض
        public int TasksCompleted { get; set; } = 0;
    }
}