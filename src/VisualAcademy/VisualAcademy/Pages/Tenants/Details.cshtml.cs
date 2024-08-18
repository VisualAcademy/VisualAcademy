namespace VisualAcademy.Pages.Tenants;

public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DetailsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public TenantModel TenantModel { get; set; }

    public async Task<IActionResult> OnGetAsync(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        TenantModel = await _context.Tenants.FirstOrDefaultAsync(m => m.Id == id);

        if (TenantModel == null)
        {
            return NotFound();
        }
        return Page();
    }
}
