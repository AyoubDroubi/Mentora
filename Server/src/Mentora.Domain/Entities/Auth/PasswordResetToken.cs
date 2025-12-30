using Mentora.Domain.Common;

namespace Mentora.Domain.Entities.Auth
{
    public class PasswordResetToken : BaseEntity
    {
        public Guid UserId { get; set; } // تغيير من string لـ Guid
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool Used { get; set; } = false;
    }
}