namespace VisualAcademy.Pages.Tenants;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<EditModel> _logger;

    public EditModel(ApplicationDbContext context, ILogger<EditModel> logger)
    {
        _context = context;
        _logger = logger ?? Microsoft.Extensions.Logging.Abstractions.NullLogger<EditModel>.Instance;
    }

    [BindProperty]
    public TenantModel TenantModel { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(long? id)
    {
        _logger.LogInformation("OnGetAsync 메서드가 호출되었습니다.");

        if (id is null)
        {
            return NotFound();
        }

        var tenant = await _context.Tenants.FirstOrDefaultAsync(m => m.Id == id);

        if (tenant is null)
        {
            return NotFound();
        }

        TenantModel = tenant;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(TenantModel).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TenantModelExists(TenantModel.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToPage("./Index");
    }

    private bool TenantModelExists(long id)
    {
        return _context.Tenants.Any(e => e.Id == id);
    }
}