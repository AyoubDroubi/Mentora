using Mentora.Domain.Common;
using Mentora.Domain.Entities.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentora.Domain.Entities
{
    public class UserStats : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public int TotalXP { get; set; } = 0;
        public int Level { get; set; } = 1;
        public int CurrentStreak { get; set; } = 0;
        public int TasksCompleted { get; set; } = 0;
    }
}