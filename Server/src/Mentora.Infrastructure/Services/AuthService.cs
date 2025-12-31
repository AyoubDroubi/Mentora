using Mentora.Application.DTOs;
using Mentora.Application.Interfaces;
using Mentora.Application.Validators;
using Mentora.Domain.Entities;
using Mentora.Domain.Entities.Auth;
using Mentora.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace Mentora.Infrastructure.Services
{
    /// <summary>
    /// Authentication service per SRS Module 1: Identity, Authentication & Security
    /// Implements BCrypt hashing, JWT tokens, and refresh token management
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ITokenService tokenService,
            ApplicationDbContext context,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _context = context;
            _config = config;
        }

        /// <summary>
        /// User Registration per SRS 1.1
        /// 1.1.1: Validates credentials via Regex
        /// 1.1.2: Checks email uniqueness (returns 409 Conflict)
        /// 1.1.3: Uses BCrypt for password hashing
        /// </summary>
        public async Task<AuthResponseDto> RegisterAsync(SignupDTO dto)
        {
            // 1.1.1: Credential Validation
            if (!PasswordValidator.IsValid(dto.Password))
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Registration failed",
                    Errors = new[] { PasswordValidator.GetValidationMessage() }
                };
            }

            // 1.1.2: Uniqueness Check
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User already exists",
                    Errors = new[] { "A user with this email already exists" }
                };
            }

            var user = new User
            {
                Id = Guid.NewGuid(), // Per SRS 8.1: GUID utilization
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            // 1.1.3: BCrypt hashing handled by Identity
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Registration failed",
                    Errors = result.Errors.Select(e => e.Description)
                };
            }

            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "User registered successfully"
            };
        }

        /// <summary>
        /// User Login per SRS 1.2
        /// 1.2.1: Issues JWT Access Token with claims
        /// 1.2.2: Generates and stores Refresh Token with device info
        /// </summary>
        public async Task<AuthResponseDto> LoginAsync(LoginDTO dto, string? ipAddress)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !user.IsActive)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid credentials"
                };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid credentials"
                };
            }

            // Update last login
            user.LastLoginAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            // 1.2.1: Generate JWT Access Token
            var accessToken = _tokenService.GenerateAccessToken(user);

            // 1.2.2: Generate and store Refresh Token
            var refreshToken = _tokenService.GenerateRefreshToken(user.Id, dto.DeviceInfo, ipAddress);
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Login successful",
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                TokenExpiration = DateTime.UtcNow.AddMinutes(60)
            };
        }

        /// <summary>
        /// Refresh Token per SRS 1.2.2
        /// Validates refresh token and issues new access token
        /// </summary>
        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken, string? ipAddress)
        {
            var token = await _tokenService.ValidateRefreshTokenAsync(refreshToken);
            if (token == null)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid or expired refresh token"
                };
            }

            var user = token.User;
            if (!user.IsActive)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User account is inactive"
                };
            }

            // Generate new access token
            var newAccessToken = _tokenService.GenerateAccessToken(user);

            // Optionally rotate refresh token (recommended security practice)
            var newRefreshToken = _tokenService.GenerateRefreshToken(user.Id, token.DeviceInfo, ipAddress);
            await _context.RefreshTokens.AddAsync(newRefreshToken);

            // Revoke old refresh token
            token.RevokedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Token refreshed successfully",
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token,
                TokenExpiration = DateTime.UtcNow.AddMinutes(60)
            };
        }

        /// <summary>
        /// Logout per SRS 1.2.3
        /// Revokes single refresh token
        /// </summary>
        public async Task<bool> LogoutAsync(string refreshToken)
        {
            await _tokenService.RevokeRefreshTokenAsync(refreshToken);
            return true;
        }

        /// <summary>
        /// Logout All Devices per SRS 1.2.3
        /// Revokes all active refresh tokens for user
        /// </summary>
        public async Task<bool> LogoutAllDevicesAsync(Guid userId)
        {
            await _tokenService.RevokeAllUserTokensAsync(userId);
            return true;
        }

        /// <summary>
        /// Forgot Password per SRS 1.2
        /// Generates password reset token and stores it
        /// </summary>
        public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                // Return true for security (don't reveal user existence)
                return true;
            }

            // Generate secure token
            var resetToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            var passwordResetToken = new PasswordResetToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = resetToken,
                ExpiresAt = DateTime.UtcNow.AddHours(1), // 1 hour validity
                Used = false
            };

            await _context.Set<PasswordResetToken>().AddAsync(passwordResetToken);
            await _context.SaveChangesAsync();

            // TODO: Send email with reset token (implement email service)
            // await _emailService.SendPasswordResetEmail(user.Email, resetToken);

            return true;
        }

        /// <summary>
        /// Reset Password per SRS 1.2
        /// Validates token and updates password
        /// </summary>
        public async Task<AuthResponseDto> ResetPasswordAsync(ResetPasswordDto dto)
        {
            // Validate password complexity
            if (!PasswordValidator.IsValid(dto.NewPassword))
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Password reset failed",
                    Errors = new[] { PasswordValidator.GetValidationMessage() }
                };
            }

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid reset request"
                };
            }

            // Validate token
            var resetToken = await _context.Set<PasswordResetToken>()
                .FirstOrDefaultAsync(t => t.Token == dto.Token && t.UserId == user.Id);

            if (resetToken == null || resetToken.Used || resetToken.ExpiresAt < DateTime.UtcNow)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid or expired reset token"
                };
            }

            // Reset password
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);

            if (!result.Succeeded)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Password reset failed",
                    Errors = result.Errors.Select(e => e.Description)
                };
            }

            // Mark token as used
            resetToken.Used = true;
            await _context.SaveChangesAsync();

            // Revoke all refresh tokens for security
            await _tokenService.RevokeAllUserTokensAsync(user.Id);

            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Password reset successfully"
            };
        }
    }
}