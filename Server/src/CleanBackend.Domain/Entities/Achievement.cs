using System.ComponentModel.DataAnnotations;

namespace Mentora.Domain.Entities
{
    public class Achievement
    {
        [Key]
        public int Id { get; set; } // 1, 2, 3 simple ID

        [Required]
        public string Name { get; set; } = string.Empty; // e.g. "Early Bird"

        public string Description { get; set; } = string.Empty;
        public string IconKey { get; set; } = string.Empty; // Front-end icon mapping
        public int XpReward { get; set; } = 50;
    }
}