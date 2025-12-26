using Mentora.Domain.Common;
using Mentora.Domain.Entities.Auth;
using System.ComponentModel.DataAnnotations;

namespace Mentora.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresOn { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedOn { get; set; }
        public bool IsActive => RevokedOn == null && DateTime.UtcNow <= ExpiresOn;
    }
}