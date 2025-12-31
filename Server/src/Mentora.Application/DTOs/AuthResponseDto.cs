namespace Mentora.Application.DTOs
{
    /// <summary>
    /// Standard authentication response per SRS 1.1
    /// </summary>
    public class AuthResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? TokenExpiration { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
