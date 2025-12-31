using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Mentora.Application.Interfaces;
using Mentora.Application.DTOs;
using System.Security.Claims;

namespace Mentora.Api.Controllers
{
    /// <summary>
    /// Authentication controller per SRS Module 1
    /// Provides endpoints for registration, login, token refresh, logout, and password reset
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Register new user - SRS 1.1
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] SignupDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(dto);
            
            if (!result.IsSuccess)
                return result.Message == "User already exists" 
                    ? Conflict(result) 
                    : BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Login user - SRS 1.2.1 and 1.2.2
        /// Returns JWT Access Token and Refresh Token
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var result = await _authService.LoginAsync(dto, ipAddress);

            if (!result.IsSuccess)
                return Unauthorized(result);

            return Ok(result);
        }

        /// <summary>
        /// Refresh access token - SRS 1.2.2
        /// </summary>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var result = await _authService.RefreshTokenAsync(dto.RefreshToken, ipAddress);

            if (!result.IsSuccess)
                return Unauthorized(result);

            return Ok(result);
        }

        /// <summary>
        /// Logout from current device - SRS 1.2.3
        /// </summary>
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LogoutAsync(dto.RefreshToken);
            return Ok(new { message = "Logged out successfully" });
        }

        /// <summary>
        /// Logout from all devices - SRS 1.2.3
        /// Revokes all refresh tokens for the user
        /// </summary>
        [HttpPost("logout-all")]
        [Authorize]
        public async Task<IActionResult> LogoutAll()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var result = await _authService.LogoutAllDevicesAsync(userId);
            return Ok(new { message = "Logged out from all devices successfully" });
        }

        /// <summary>
        /// Request password reset - SRS 1.2
        /// </summary>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _authService.ForgotPasswordAsync(dto);
            
            // Always return success for security (don't reveal user existence)
            return Ok(new { message = "If the email exists, a password reset link has been sent" });
        }

        /// <summary>
        /// Reset password - SRS 1.2
        /// </summary>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.ResetPasswordAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Get current user info
        /// </summary>
        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirst("userId")?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var firstName = User.FindFirst("firstName")?.Value;
            var lastName = User.FindFirst("lastName")?.Value;

            return Ok(new
            {
                userId,
                email,
                firstName,
                lastName
            });
        }
    }
}