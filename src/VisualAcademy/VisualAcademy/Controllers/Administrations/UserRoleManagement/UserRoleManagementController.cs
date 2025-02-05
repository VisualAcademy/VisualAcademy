using Microsoft.AspNetCore.Mvc;

namespace VisualAcademy.Controllers.Administrations.UserRoleManagement
{
    public class UserRoleManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
