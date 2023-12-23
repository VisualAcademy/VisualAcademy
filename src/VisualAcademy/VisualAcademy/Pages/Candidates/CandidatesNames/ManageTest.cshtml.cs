using VisualAcademy.Areas.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VisualAcademy.Pages.Candidates.CandidatesNames
{
    public class ManageTestModel(UserManager<ApplicationUser> userManager) : PageModel
    {
        public string UserId { get; set; }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userId = await userManager.GetUserIdAsync(user);

            UserId = userId; 
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }
    }
}
