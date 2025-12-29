using Microsoft.AspNetCore.Mvc;
using Mentora.Application.Interfaces;
using Mentora.Application.DTOs;

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

    }
}