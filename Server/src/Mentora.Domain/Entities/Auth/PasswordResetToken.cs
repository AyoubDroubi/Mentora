using CleanBackend.Domain.Common;

namespace CleanBackend.Domain.Entities.Auth
{
    public class PasswordResetToken : BaseEntity
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool Used { get; set; } = false;
    }
}