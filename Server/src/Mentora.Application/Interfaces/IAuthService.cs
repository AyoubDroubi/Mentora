using Mentora.Application.DTOs;
using Mentora.Domain.Entities.Auth;

namespace Mentora.Application.Interfaces
{
    /// <summary>
    /// Authentication service interface per SRS 1.1 and 1.2
    /// </summary>
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(SignupDTO dto);

        Task<AuthResponseDto> LoginAsync(LoginDTO dto, string? ipAddress);

        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken, string? ipAddress);

        Task<bool> LogoutAsync(string refreshToken);

        Task<bool> LogoutAllDevicesAsync(Guid userId);

        Task<bool> ForgotPasswordAsync(ForgotPasswordDto dto);

        Task<AuthResponseDto> ResetPasswordAsync(ResetPasswordDto dto);
    }
}