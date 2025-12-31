using System.ComponentModel.DataAnnotations;

namespace Mentora.Application.DTOs
{
    /// <summary>
    /// Refresh token request per SRS 1.2.2
    /// </summary>
    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
