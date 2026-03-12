using Microsoft.AspNetCore.Mvc;

namespace VisualAcademy.Controllers
{
    public class EmployeePhotosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
