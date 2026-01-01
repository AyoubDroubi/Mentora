using Microsoft.AspNetCore.Mvc;

namespace Mentora.Api.Controllers
{
    public class BaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
