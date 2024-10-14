namespace VisualAcademy.Pages.Tenants;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<EditModel> _logger;

    public EditModel(ApplicationDbContext context, ILogger<EditModel> logger)
    {
        _context = context;

        // ���� logger�� null�̸� NullLogger �ν��Ͻ��� ����Ͽ� ���� ���� 
        _logger = logger ?? Microsoft.Extensions.Logging.Abstractions.NullLogger<EditModel>.Instance;
    }

    [BindProperty]
    public TenantModel TenantModel { get; set; }

    public async Task<IActionResult> OnGetAsync(long? id)
    {
        // �α�: GET ��û�� ���۵��� �˸�
        _logger.LogInformation("OnGetAsync �޼��尡 ȣ��Ǿ����ϴ�."); 

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
