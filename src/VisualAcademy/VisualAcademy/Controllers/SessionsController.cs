using Microsoft.AspNetCore.Mvc;

namespace VisualAcademy.Controllers
{
    public class SessionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
