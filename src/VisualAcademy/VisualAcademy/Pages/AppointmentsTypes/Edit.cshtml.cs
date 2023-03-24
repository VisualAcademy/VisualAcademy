using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using VisualAcademy.Models.Tenants;
using VisualAcademy.Repositories.Tenants;

namespace VisualAcademy.Pages.AppointmentsTypes {
    public class EditModel : PageModel {
        private readonly IAppointmentTypeRepository _appointmentTypeRepository;

        public EditModel(IAppointmentTypeRepository appointmentTypeRepository) => _appointmentTypeRepository = appointmentTypeRepository;

        [BindProperty]
        public AppointmentTypeModel AppointmentType { get; set; }

        public async Task<IActionResult> OnGetAsync(long id) {
            long tenantId = 1; // tenantId ���� 1�� �ʱ�ȭ�մϴ�.
            AppointmentType = await _appointmentTypeRepository.GetByIdAsync(id, tenantId);

            if (AppointmentType == null) {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            long tenantId = 1; // tenantId ���� 1�� �ʱ�ȭ�մϴ�.
            AppointmentType.TenantId = tenantId; 
            await _appointmentTypeRepository.UpdateAsync(AppointmentType);

            return RedirectToPage("./Index");
        }
    }
}
