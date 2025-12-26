using Microsoft.AspNetCore.Mvc;
using Mentora.Domain.DTOs.Auth;

namespace Mentora.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(SignupDTO dto)
        {
            var result = await _authService.RegisterAsync(dto);
            if (!result.IsSuccess) return BadRequest(result.Errors);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var token = await _authService.LoginAsync(dto);
            if (string.IsNullOrEmpty(token)) return Unauthorized("Invalid credentials");
            return Ok(new { token });
        }
    }
}