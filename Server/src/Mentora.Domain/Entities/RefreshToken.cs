using Mentora.Domain.Common;
using Mentora.Domain.Entities.Auth;

namespace Mentora.Domain.Entities
{
    /// <summary>
    /// RefreshToken entity per SRS 1.2.2: Persistent Sessions
    /// Supports device tracking and selective token revocation
    /// </summary>
    public class RefreshToken : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresOn { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedOn { get; set; }
        
        // Device Information for "Logout All Devices" feature per SRS 1.2.3
        public string? DeviceInfo { get; set; }
        public string? IpAddress { get; set; }
        
        public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
        public bool IsRevoked => RevokedOn != null;
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}