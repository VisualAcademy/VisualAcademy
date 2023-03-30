using Microsoft.AspNetCore.Mvc.RazorPages;
using VisualAcademy.Models.Tenants;
using VisualAcademy.Repositories.Tenants;

namespace VisualAcademy.Pages.AppointmentsTypes; 
public class IndexModel : PageModel {
    private readonly IAppointmentTypeRepository _appointmentTypeRepository;

    public IndexModel(IAppointmentTypeRepository appointmentTypeRepository) {
        _appointmentTypeRepository = appointmentTypeRepository;
    }

    public IList<AppointmentTypeModel> AppointmentTypes { get; set; }

    public async Task OnGetAsync() {
        long tenantId = 1; // tenantId ���� 1�� �ʱ�ȭ�մϴ�.
        AppointmentTypes = await _appointmentTypeRepository.GetAllAsync(tenantId);
    }
}
