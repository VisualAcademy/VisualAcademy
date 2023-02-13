#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VisualAcademy.Data;
using VisualAcademy.Models;

namespace VisualAcademy.Pages.Administrations.Properties;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context) => _context = context;

    [BindProperty]
    public Property Property { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Property = await _context.Properties.FirstOrDefaultAsync(m => m.Id == id);

        if (Property == null)
        {
            return NotFound();
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Property = await _context.Properties.FindAsync(id);

        if (Property != null)
        {
            _context.Properties.Remove(Property);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
