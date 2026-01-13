using Microsoft.AspNetCore.Mvc;

namespace VisualAcademy.Controllers
{
    public class UserSignaturesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
