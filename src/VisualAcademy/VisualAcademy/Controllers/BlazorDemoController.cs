using Microsoft.AspNetCore.Mvc;

namespace VisualAcademy.Controllers {
    public class BlazorDemoController : Controller {
        public IActionResult Index() => View();
    }
}
