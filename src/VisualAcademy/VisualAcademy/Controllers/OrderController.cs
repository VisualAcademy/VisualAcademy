using Microsoft.AspNetCore.Mvc;

namespace VisualAcademy.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
