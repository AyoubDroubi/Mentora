using System.ComponentModel.DataAnnotations;

namespace Mentora.Application.DTOs
{
    /// <summary>
    /// Password reset request DTO per SRS 1.2
    /// </summary>
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;
    }
}
