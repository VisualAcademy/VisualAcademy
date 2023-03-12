using VisualAcademy.Models.Tenants;

namespace VisualAcademy.Repositories.Tenants {
    public interface IAppointmentTypeRepository {
        Task<List<AppointmentTypeModel>> GetAllAsync(long tenantId);
        Task<AppointmentTypeModel> GetByIdAsync(long id, long tenantId);
        Task AddAsync(AppointmentTypeModel appointmentType);
        Task UpdateAsync(AppointmentTypeModel appointmentType);
        Task DeleteAsync(long id, long tenantId);
    }
}
