using Mentora.Domain.Entities.Auth;
using Mentora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Mentora.Infrastructure.BackgroundServices
{
    /// <summary>
    /// Background service to cleanup expired password reset tokens
    /// Runs daily to maintain database hygiene
    /// </summary>
    public class TokenCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TokenCleanupService> _logger;
        private readonly TimeSpan _cleanupInterval = TimeSpan.FromHours(24);

        public TokenCleanupService(IServiceProvider serviceProvider, ILogger<TokenCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Token Cleanup Service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CleanupExpiredTokensAsync();
                    await Task.Delay(_cleanupInterval, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while cleaning up expired tokens");
                    await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Retry after 1 hour on error
                }
            }
        }

        private async Task CleanupExpiredTokensAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var cutoffDate = DateTime.UtcNow.AddDays(-7); // Keep tokens for 7 days after expiry

            // Cleanup expired password reset tokens
            var expiredPasswordTokens = await context.Set<PasswordResetToken>()
                .Where(t => t.ExpiresAt < cutoffDate || (t.Used && t.UpdatedAt < cutoffDate))
                .ToListAsync();

            if (expiredPasswordTokens.Any())
            {
                context.Set<PasswordResetToken>().RemoveRange(expiredPasswordTokens);
                await context.SaveChangesAsync();
                _logger.LogInformation($"Cleaned up {expiredPasswordTokens.Count} expired password reset tokens");
            }

            // Cleanup expired refresh tokens
            var expiredRefreshTokens = await context.RefreshTokens
                .Where(t => t.ExpiresOn < cutoffDate || (t.RevokedOn != null && t.RevokedOn < cutoffDate))
                .ToListAsync();

            if (expiredRefreshTokens.Any())
            {
                context.RefreshTokens.RemoveRange(expiredRefreshTokens);
                await context.SaveChangesAsync();
                _logger.LogInformation($"Cleaned up {expiredRefreshTokens.Count} expired refresh tokens");
            }
        }
    }
}
