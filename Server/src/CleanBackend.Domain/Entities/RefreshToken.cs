using System.ComponentModel.DataAnnotations;

namespace Mentora.Domain.Entities
{
    public class RefreshToken
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresOn { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedOn { get; set; } // اذا عملنا تسجيل خروج قسري

        // هل التوكن فعال؟ (لم تنته صلاحيته ولم يتم إلغاؤه)
        public bool IsActive => RevokedOn == null && DateTime.UtcNow <= ExpiresOn;
    }
}