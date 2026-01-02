using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities;
using Mentora.Domain.Entities.Auth;
using Mentora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for RefreshToken entity
    /// Handles all database operations for refresh tokens
    /// </summary>
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Token == token);
        }

        public async Task<RefreshToken?> GetActiveTokenByUserIdAsync(Guid userId, string deviceInfo)
        {
            return await _context.RefreshTokens
                .Where(t => t.UserId == userId && 
                           t.DeviceInfo == deviceInfo && 
                           t.RevokedOn == null && 
                           t.ExpiresOn > DateTime.UtcNow)
                .OrderByDescending(t => t.CreatedOn)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId)
        {
            return await _context.RefreshTokens
                .Where(t => t.UserId == userId && t.IsActive)
                .ToListAsync();
        }

        public async Task<RefreshToken> CreateAsync(RefreshToken token)
        {
            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();
            return token;
        }

        public async Task UpdateAsync(RefreshToken token)
        {
            _context.RefreshTokens.Update(token);
            await _context.SaveChangesAsync();
        }

        public async Task RevokeTokenAsync(string token)
        {
            var refreshToken = await GetByTokenAsync(token);
            if (refreshToken != null && !refreshToken.IsRevoked)
            {
                refreshToken.RevokedOn = DateTime.UtcNow;
                await UpdateAsync(refreshToken);
            }
        }

        public async Task RevokeAllUserTokensAsync(Guid userId)
        {
            var tokens = await GetActiveTokensByUserIdAsync(userId);
            foreach (var token in tokens)
            {
                token.RevokedOn = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
        }

        public async Task CleanupExpiredTokensAsync()
        {
            var expiredTokens = await _context.RefreshTokens
                .Where(t => t.IsExpired)
                .ToListAsync();

            _context.RefreshTokens.RemoveRange(expiredTokens);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Repository implementation for PasswordResetToken entity
    /// Handles all database operations for password reset tokens
    /// </summary>
    public class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public PasswordResetTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PasswordResetToken?> GetByTokenAsync(string token)
        {
            return await _context.Set<PasswordResetToken>()
                .FirstOrDefaultAsync(t => t.Token == token);
        }

        public async Task<PasswordResetToken?> GetActiveTokenByUserIdAsync(Guid userId)
        {
            return await _context.Set<PasswordResetToken>()
                .Where(t => t.UserId == userId && 
                           !t.Used && 
                           t.ExpiresAt > DateTime.UtcNow)
                .OrderByDescending(t => t.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<PasswordResetToken> CreateAsync(PasswordResetToken token)
        {
            await _context.Set<PasswordResetToken>().AddAsync(token);
            await _context.SaveChangesAsync();
            return token;
        }

        public async Task UpdateAsync(PasswordResetToken token)
        {
            _context.Set<PasswordResetToken>().Update(token);
            await _context.SaveChangesAsync();
        }

        public async Task CleanupExpiredTokensAsync()
        {
            var expiredTokens = await _context.Set<PasswordResetToken>()
                .Where(t => t.ExpiresAt < DateTime.UtcNow || t.Used)
                .ToListAsync();

            _context.Set<PasswordResetToken>().RemoveRange(expiredTokens);
            await _context.SaveChangesAsync();
        }
    }
}
