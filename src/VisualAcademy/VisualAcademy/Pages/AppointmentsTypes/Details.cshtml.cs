using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisualAcademy.Models.Tenants;
using VisualAcademy.Repositories.Tenants;

namespace VisualAcademy.Pages.AppointmentsTypes {
    public class DetailsModel : PageModel {
        private readonly IAppointmentTypeRepository _appointmentTypeRepository;

        public DetailsModel(IAppointmentTypeRepository appointmentTypeRepository) {
            _appointmentTypeRepository = appointmentTypeRepository;
        }

        public AppointmentTypeModel AppointmentType { get; set; }

        public async Task<IActionResult> OnGetAsync(long id) {
            var tenantId = 1; // tenantId 값을 1로 초기화합니다.

            AppointmentType = await _appointmentTypeRepository.GetByIdAsync(id, tenantId);

            if (AppointmentType == null) {
                return NotFound();
            }

            return Page();
        }
    }
}
