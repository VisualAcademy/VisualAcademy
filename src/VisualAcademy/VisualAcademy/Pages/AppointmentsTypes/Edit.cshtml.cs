using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisualAcademy.Models.Tenants;
using VisualAcademy.Repositories.Tenants;

namespace VisualAcademy.Pages.AppointmentsTypes
{
    public class EditModel : PageModel
    {
        private readonly IAppointmentTypeRepository _appointmentTypeRepository;

        public EditModel(IAppointmentTypeRepository appointmentTypeRepository)
        {
            _appointmentTypeRepository = appointmentTypeRepository;
        }

        [BindProperty]
        public AppointmentTypeModel? AppointmentType { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            const long tenantId = 1;

            var appointmentType = await _appointmentTypeRepository.GetByIdAsync(id, tenantId);

            if (appointmentType is null)
            {
                return NotFound();
            }

            AppointmentType = appointmentType;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (AppointmentType is null)
            {
                return NotFound();
            }

            const long tenantId = 1;
            AppointmentType.TenantId = tenantId;

            await _appointmentTypeRepository.UpdateAsync(AppointmentType);

            return RedirectToPage("./Index");
        }
    }
}