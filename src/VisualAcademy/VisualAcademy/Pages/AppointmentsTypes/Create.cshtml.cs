using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisualAcademy.Models.Tenants;
using VisualAcademy.Repositories.Tenants;

namespace VisualAcademy.Pages.AppointmentsTypes;

public class CreateModel : PageModel
{
    private readonly IAppointmentTypeRepository _appointmentTypeRepository;

    public CreateModel(IAppointmentTypeRepository appointmentTypeRepository)
    {
        _appointmentTypeRepository = appointmentTypeRepository;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

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

        await _appointmentTypeRepository.AddAsync(AppointmentType);

        return RedirectToPage("./Index");
    }
}
