using Microsoft.AspNetCore.Mvc;

namespace VisualAcademy.Controllers
{
    public class ArticlesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
