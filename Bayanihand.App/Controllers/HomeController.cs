using Microsoft.AspNetCore.Mvc;

namespace Bayanihand.App.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
