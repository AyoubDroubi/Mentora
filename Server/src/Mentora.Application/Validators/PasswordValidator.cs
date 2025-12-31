using System.Text.RegularExpressions;

namespace Mentora.Application.Validators
{
    /// <summary>
    /// Validates password complexity: min 8 chars, 1 uppercase, 1 special char
    /// Per SRS 1.1.1: Credential Validation
    /// </summary>
    public static class PasswordValidator
    {
        private static readonly Regex PasswordRegex = new Regex(
            @"^(?=.*[A-Z])(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]).{8,}$",
            RegexOptions.Compiled
        );

        public static bool IsValid(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            return PasswordRegex.IsMatch(password);
        }

        public static string GetValidationMessage()
        {
            return "Password must be at least 8 characters long, contain at least 1 uppercase letter, and 1 special character.";
        }
    }
}
