using Microsoft.AspNetCore.Mvc;

namespace Coworking.MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
