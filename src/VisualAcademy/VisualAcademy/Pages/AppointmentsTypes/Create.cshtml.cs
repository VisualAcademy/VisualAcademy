using VisualAcademy.Models.Tenants;
using VisualAcademy.Repositories.Tenants;

namespace VisualAcademy.Pages.AppointmentsTypes;

public class CreateModel(IAppointmentTypeRepository appointmentTypeRepository) : PageModel
{
    public IActionResult OnGet() => Page();

    [BindProperty]
    public AppointmentTypeModel AppointmentType { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        long tenantId = 1; // tenantId ���� 1�� �ʱ�ȭ�մϴ�.
        AppointmentType.TenantId = tenantId;

        if (!ModelState.IsValid)
        {
            return Page();
        }

        await appointmentTypeRepository.AddAsync(AppointmentType);

        return RedirectToPage("./Index");
    }
}
