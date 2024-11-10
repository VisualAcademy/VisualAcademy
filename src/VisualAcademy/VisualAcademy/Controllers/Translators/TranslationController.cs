using Microsoft.AspNetCore.Mvc;

namespace VisualAcademy.Controllers.Translators
{
    public class TranslationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
