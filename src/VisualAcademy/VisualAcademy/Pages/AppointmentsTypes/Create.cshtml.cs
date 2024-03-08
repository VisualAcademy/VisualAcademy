using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        long tenantId = 1; // tenantId 값을 1로 초기화합니다.
        AppointmentType.TenantId = tenantId;

        if (!ModelState.IsValid)
        {
            return Page();
        }

        await appointmentTypeRepository.AddAsync(AppointmentType);

        return RedirectToPage("./Index");
    }
}
