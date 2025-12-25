using CleanBackend.Application.DTOs;
using System.Threading.Tasks;

namespace CleanBackend.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(SignupDTO dto); 
        Task<string> LoginAsync(LoginDTO dto);
        Task<bool> ForgotPasswordAsync(string email);
        Task<string> ResetPasswordAsync(ResetPasswordDto dto);
    }

    // AuthResponseDto بسيط جداً (ضعيه في مجلد الـ DTOs)
    public class AuthResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}