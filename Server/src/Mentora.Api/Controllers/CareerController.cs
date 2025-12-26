//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//namespace Mentora.Api.Controllers
//{
//    [Authorize]
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CareerController : ControllerBase
//    {
//        private readonly ICareerService _careerService;

//        public CareerController(ICareerService careerService)
//        {
//            _careerService = careerService;
//        }

//        // Helper to get ID from token
//        private string GetCurrentUserId() => User.FindFirst("userId")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//        [HttpPost]
//        public async Task<IActionResult> Create(CareerPlanCreateDto dto)
//        {
//            var userId = GetCurrentUserId();
//            var result = await _careerService.CreateAsync(userId, dto);
//            return Ok(result);
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            var plans = await _careerService.GetAllAsync(GetCurrentUserId());
//            return Ok(plans);
//        }

//        // وباقي الـ Update و Delete بنفس الطريقة
//    }
//}