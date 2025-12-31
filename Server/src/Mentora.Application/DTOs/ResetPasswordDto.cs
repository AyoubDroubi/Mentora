using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs
{
    /// <summary>
    /// Password reset DTO per SRS 1.2
    /// </summary>
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Reset token is required")]
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "New password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]).{8,}$",
            ErrorMessage = "Password must contain at least 1 uppercase letter and 1 special character")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password confirmation is required")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}