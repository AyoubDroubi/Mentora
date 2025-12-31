using Mentora.Application.Interfaces;
using Mentora.Domain.Entities;
using Mentora.Domain.Entities.Auth;
using Mentora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Mentora.Infrastructure.Services
{
    /// <summary>
    /// Token service implementation per SRS 1.2.1 and 1.2.2
    /// Generates signed JWT tokens and cryptographically secure refresh tokens
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;

        public TokenService(IConfiguration config, ApplicationDbContext context)
        {
            _config = config;
            _context = context;
        }

        /// <summary>
        /// Generates JWT Access Token per SRS 1.2.1
        /// Contains Claims: UserId, Role, Email with 60-minute expiration
        /// </summary>
        public string GenerateAccessToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured"))
            );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim("userId", user.Id.ToString()),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60), // 60 minutes per SRS
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Generates cryptographically secure refresh token per SRS 1.2.2
        /// Stores device info for "Logout All Devices" feature
        /// </summary>
        public RefreshToken GenerateRefreshToken(Guid userId, string? deviceInfo, string? ipAddress)
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(30), // 30 days validity
                CreatedOn = DateTime.UtcNow,
                DeviceInfo = deviceInfo,
                IpAddress = ipAddress
            };
        }

        public async Task<RefreshToken?> ValidateRefreshTokenAsync(string token)
        {
            var refreshToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token);

            if (refreshToken == null || !refreshToken.IsActive)
                return null;

            return refreshToken;
        }

        public async Task RevokeRefreshTokenAsync(string token)
        {
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token);

            if (refreshToken != null)
            {
                refreshToken.RevokedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Logout All Devices per SRS 1.2.3
        /// Flags all active refresh tokens as revoked
        /// </summary>
        public async Task RevokeAllUserTokensAsync(Guid userId)
        {
            var tokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && rt.RevokedOn == null)
                .ToListAsync();

            foreach (var token in tokens)
            {
                token.RevokedOn = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }
    }
}
