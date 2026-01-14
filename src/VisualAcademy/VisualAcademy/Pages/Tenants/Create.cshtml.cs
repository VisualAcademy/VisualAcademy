namespace VisualAcademy.Pages.Tenants
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TenantModel TenantModel { get; set; } = default!;

        public IActionResult OnGet()
        {
            TenantModel = new TenantModel();
            return Page();
        }

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
