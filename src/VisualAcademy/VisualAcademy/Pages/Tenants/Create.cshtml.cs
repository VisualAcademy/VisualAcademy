using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisualAcademy.Data;
using VisualAcademy.Models;

namespace VisualAcademy.Pages.Tenants
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context) => _context = context;

        public IActionResult OnGet() => Page();

        [BindProperty]
        public TenantModel TenantModel { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Tenants.Add(TenantModel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
