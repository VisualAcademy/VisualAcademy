#nullable disable
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VisualAcademy.Pages.Administrations.Properties;

public class CreateModel(ApplicationDbContext context) : PageModel
{
    public IActionResult OnGet() => Page();

    [BindProperty]
    public Property Property { get; set; }

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        context.Properties.Add(Property);
        await context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
