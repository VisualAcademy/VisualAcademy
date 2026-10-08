using Microsoft.AspNetCore.Mvc;

namespace VisualAcademy.Controllers
{
    public class CareersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
