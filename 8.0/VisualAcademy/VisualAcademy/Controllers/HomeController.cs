using Microsoft.AspNetCore.Mvc;

namespace VisualAcademy.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Hello()
        {
            return Content("<h1>안녕하세요. MVC!</h1>", "text/html; charset=utf-8");
        }
    }
}
