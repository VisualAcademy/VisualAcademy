namespace VisualAcademy.Pages.Candidates.CandidatesNames
{
    public class ManageTestModel(UserManager<ApplicationUser> userManager) : PageModel
    {
        public string UserId { get; set; } = string.Empty;

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