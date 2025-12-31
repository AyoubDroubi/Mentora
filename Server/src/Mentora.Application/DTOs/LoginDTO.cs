using System.ComponentModel.DataAnnotations;

namespace Mentora.Application.DTOs
{
    /// <summary>
    /// User login DTO per SRS 1.2.1: Token Issuance
    /// </summary>
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; set; } = string.Empty;
        
        // Optional device tracking
        public string? DeviceInfo { get; set; }
    }
}