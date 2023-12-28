using Microsoft.AspNetCore.Mvc;

namespace VisualAcademy.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
