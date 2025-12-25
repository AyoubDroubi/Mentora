using System.ComponentModel.DataAnnotations;

namespace Mentora.Domain.Entities
{
    public class Notification
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; } // User Navigation removed for brevity, usually needed

        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;

        // Type: "Reminder", "Alert", "Achievement"
        public string Type { get; set; } = "System";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}