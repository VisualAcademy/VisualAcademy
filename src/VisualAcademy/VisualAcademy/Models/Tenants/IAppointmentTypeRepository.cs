using System.Collections.Generic;
using System.Threading.Tasks;
using VisualAcademy.Models.Tenants;

namespace VisualAcademy.Models.Tenants {
    public interface IAppointmentTypeRepository {
        Task<List<AppointmentTypeModel>> GetAllAsync();
        Task<AppointmentTypeModel> GetByIdAsync(long id);
        Task CreateAsync(AppointmentTypeModel appointmentType);
        Task UpdateAsync(AppointmentTypeModel appointmentType);
        Task DeleteAsync(long id);
    }
}
