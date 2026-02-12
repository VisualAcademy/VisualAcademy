namespace VisualAcademy.Pages.Tenants;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<EditModel> _logger;

    public EditModel(ApplicationDbContext context, ILogger<EditModel> logger)
    {
        _context = context;

        // 만약 logger가 null이면 NullLogger 인스턴스를 사용하여 예외 방지 
        _logger = logger ?? Microsoft.Extensions.Logging.Abstractions.NullLogger<EditModel>.Instance;
    }

    [BindProperty]
    public TenantModel TenantModel { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(long? id)
    {
        // 로깅: GET 요청이 시작됨을 알림
        _logger.LogInformation("OnGetAsync 메서드가 호출되었습니다."); 

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
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool TenantModelExists(long id)
    {
        return _context.Tenants.Any(e => e.Id == id);
    }
}
