using Mentora.Domain.Entities;
using Mentora.Domain.Entities.Auth;

namespace Mentora.Application.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for RefreshToken entity
    /// Handles data access for refresh token management
    /// </summary>
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token);

        Task<RefreshToken?> GetActiveTokenByUserIdAsync(Guid userId, string deviceInfo);

        Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId);

        Task<RefreshToken> CreateAsync(RefreshToken token);

        Task UpdateAsync(RefreshToken token);

        Task RevokeTokenAsync(string token);

        Task RevokeAllUserTokensAsync(Guid userId);

        Task CleanupExpiredTokensAsync();
    }

    /// <summary>
    /// Repository interface for PasswordResetToken entity
    /// Handles data access for password reset token management
    /// </summary>
    public interface IPasswordResetTokenRepository
    {
        Task<PasswordResetToken?> GetByTokenAsync(string token);

        Task<PasswordResetToken?> GetActiveTokenByUserIdAsync(Guid userId);

        Task<PasswordResetToken> CreateAsync(PasswordResetToken token);

        Task UpdateAsync(PasswordResetToken token);

        Task CleanupExpiredTokensAsync();
    }
}