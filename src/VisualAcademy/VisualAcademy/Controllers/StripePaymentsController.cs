using Microsoft.AspNetCore.Mvc;

namespace VisualAcademy.Controllers
{
    public class StripePaymentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
