using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Mentora.Api.Controllers
{
    /// <summary>
    /// Base controller providing common functionality for all API controllers
    /// </summary>
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// Get the authenticated user's ID from JWT claims
        /// </summary>
        /// <returns>User ID as Guid</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when user is not authenticated</exception>
        protected Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new UnauthorizedAccessException("User ID not found in token");
            }

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("Invalid user ID format");
            }

            return userId;
        }

        /// <summary>
        /// Get the authenticated user's email from JWT claims
        /// </summary>
        /// <returns>User email</returns>
        protected string GetUserEmail()
        {
            return User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
        }

        /// <summary>
        /// Check if the current user is in a specific role
        /// </summary>
        /// <param name="role">Role name to check</param>
        /// <returns>True if user is in role, false otherwise</returns>
        protected bool IsInRole(string role)
        {
            return User.IsInRole(role);
        }
    }
}
