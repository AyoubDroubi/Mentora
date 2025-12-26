using CleanBackend.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Mentora.Domain.Entities
{
    public class Achievement : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public string IconKey { get; set; } = string.Empty;
        public int XpReward { get; set; } = 50;
    }
}