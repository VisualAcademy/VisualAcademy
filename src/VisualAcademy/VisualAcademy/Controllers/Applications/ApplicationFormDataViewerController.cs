using Microsoft.AspNetCore.Mvc;

namespace VisualAcademy.Controllers.Applications
{
    public class ApplicationFormDataViewerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
