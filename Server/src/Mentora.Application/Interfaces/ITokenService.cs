using Mentora.Domain.Entities;
using Mentora.Domain.Entities.Auth;

namespace Mentora.Application.Interfaces
{
    /// <summary>
    /// Token service interface per SRS 1.2.1 and 1.2.2
    /// Handles JWT Access Token and Refresh Token generation
    /// </summary>
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        RefreshToken GenerateRefreshToken(Guid userId, string? deviceInfo, string? ipAddress);
        Task<RefreshToken?> ValidateRefreshTokenAsync(string token);
        Task RevokeRefreshTokenAsync(string token);
        Task RevokeAllUserTokensAsync(Guid userId);
    }
}
