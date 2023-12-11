using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VisualAcademy.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class SetPasswordConfirmationModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
