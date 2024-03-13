using Microsoft.AspNetCore.Mvc.RazorPages;
using VisualAcademy.Models.Tenants;
using VisualAcademy.Repositories.Tenants;

namespace VisualAcademy.Pages.AppointmentsTypes;

public class DeleteModel : PageModel
{
    private readonly IAppointmentTypeRepository _appointmentTypeRepository;

    public DeleteModel(IAppointmentTypeRepository appointmentTypeRepository) => _appointmentTypeRepository = appointmentTypeRepository;

    [BindProperty]
    public AppointmentTypeModel AppointmentType { get; set; }

    public long TenantId { get; set; } = 1;

    public async Task<IActionResult> OnGetAsync(long id)
    {
        TenantId = 1;
        AppointmentType = await _appointmentTypeRepository.GetByIdAsync(id, TenantId);

        if (AppointmentType == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(long id)
    {
        TenantId = 1;
        await _appointmentTypeRepository.DeleteAsync(id, TenantId);

        return RedirectToPage("./Index");
    }
}
