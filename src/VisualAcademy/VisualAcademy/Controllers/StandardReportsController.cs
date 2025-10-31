using Microsoft.AspNetCore.Mvc;

namespace VisualAcademy.Controllers
{
    public class StandardReportsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
