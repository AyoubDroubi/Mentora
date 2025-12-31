using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Mentora.Application.Interfaces;
using Mentora.Application.DTOs;
using System.Security.Claims;

namespace Mentora.Api.Controllers
{
    /// <summary>
    /// Authentication and Authorization API
    /// </summary>
    /// <remarks>
    /// Provides endpoints for user authentication, token management, and password reset operations.
    /// Implements SRS Module 1: Identity, Authentication &amp; Security
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Register a new user account
        /// </summary>
        /// <param name="dto">User registration details</param>
        /// <returns>Registration result with success status</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/auth/register
        ///     {
        ///        "firstName": "أحمد",
        ///        "lastName": "محمد",
        ///        "email": "ahmed@example.com",
        ///        "password": "Test@123"
        ///     }
        /// 
        /// Password Requirements (SRS 1.1.1):
        /// - Minimum 8 characters
        /// - At least 1 uppercase letter
        /// - At least 1 special character
        /// </remarks>
        /// <response code="200">User registered successfully</response>
        /// <response code="400">Invalid input data or password doesn't meet requirements</response>
        /// <response code="409">User with this email already exists</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status409Conflict)]
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
        /// Login to user account
        /// </summary>
        /// <param name="dto">Login credentials</param>
        /// <returns>JWT access token and refresh token</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/auth/login
        ///     {
        ///        "email": "ahmed@example.com",
        ///        "password": "Test@123",
        ///        "deviceInfo": "Chrome/Windows"
        ///     }
        /// 
        /// Returns:
        /// - Access Token: JWT token valid for 60 minutes (SRS 1.2.1)
        /// - Refresh Token: Token valid for 7 days (SRS 1.2.2)
        /// </remarks>
        /// <response code="200">Login successful with access and refresh tokens</response>
        /// <response code="400">Invalid input data</response>
        /// <response code="401">Invalid credentials</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status401Unauthorized)]
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
        /// Refresh the access token
        /// </summary>
        /// <param name="dto">Refresh token</param>
        /// <returns>New access token and refresh token</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/auth/refresh-token
        ///     {
        ///        "refreshToken": "your-refresh-token-here"
        ///     }
        /// 
        /// Implements token rotation: old refresh token is revoked and new one is issued (SRS 1.2.2)
        /// </remarks>
        /// <response code="200">Token refreshed successfully</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Invalid or expired refresh token</response>
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status401Unauthorized)]
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
        /// Logout from current device
        /// </summary>
        /// <param name="dto">Refresh token to revoke</param>
        /// <returns>Success message</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/auth/logout
        ///     Authorization: Bearer {your-access-token}
        ///     {
        ///        "refreshToken": "your-refresh-token-here"
        ///     }
        /// 
        /// Revokes the specified refresh token (SRS 1.2.3)
        /// </remarks>
        /// <response code="200">Logout successful</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Unauthorized - invalid or missing access token</response>
        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LogoutAsync(dto.RefreshToken);
            return Ok(new { message = "Logged out successfully" });
        }

        /// <summary>
        /// Logout from all devices
        /// </summary>
        /// <returns>Success message</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/auth/logout-all
        ///     Authorization: Bearer {your-access-token}
        /// 
        /// Revokes all refresh tokens for the current user across all devices (SRS 1.2.3)
        /// </remarks>
        /// <response code="200">Logged out from all devices successfully</response>
        /// <response code="401">Unauthorized - invalid or missing access token</response>
        [HttpPost("logout-all")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LogoutAll()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var result = await _authService.LogoutAllDevicesAsync(userId);
            return Ok(new { message = "Logged out from all devices successfully" });
        }

        /// <summary>
        /// Request password reset
        /// </summary>
        /// <param name="dto">Email address</param>
        /// <returns>Success message</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/auth/forgot-password
        ///     {
        ///        "email": "ahmed@example.com"
        ///     }
        /// 
        /// Always returns success for security reasons (doesn't reveal if email exists).
        /// If email exists, a reset token is generated and stored (SRS 1.2)
        /// </remarks>
        /// <response code="200">Request processed (email sent if account exists)</response>
        /// <response code="400">Invalid input</response>
        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _authService.ForgotPasswordAsync(dto);
            
            // Always return success for security (don't reveal user existence)
            return Ok(new { message = "If the email exists, a password reset link has been sent" });
        }

        /// <summary>
        /// Reset password using token
        /// </summary>
        /// <param name="dto">Password reset details</param>
        /// <returns>Success or error message</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/auth/reset-password
        ///     {
        ///        "email": "ahmed@example.com",
        ///        "token": "reset-token-from-email",
        ///        "newPassword": "NewPass@123",
        ///        "confirmPassword": "NewPass@123"
        ///     }
        /// 
        /// Token is valid for 1 hour. After successful reset, all refresh tokens are revoked (SRS 1.2)
        /// </remarks>
        /// <response code="200">Password reset successfully</response>
        /// <response code="400">Invalid input, token expired, or passwords don't match requirements</response>
        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status400BadRequest)]
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
        /// Get current authenticated user information
        /// </summary>
        /// <returns>User information from JWT token claims</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/auth/me
        ///     Authorization: Bearer {your-access-token}
        /// 
        /// Returns user information extracted from the JWT token
        /// </remarks>
        /// <response code="200">User information retrieved successfully</response>
        /// <response code="401">Unauthorized - invalid or missing access token</response>
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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