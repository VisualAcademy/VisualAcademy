using VisualAcademy.Models.Tenants;

namespace VisualAcademy.Repositories.Tenants;

public class AppointmentTypeRepository : IAppointmentTypeRepository {
    private readonly ApplicationDbContext _dbContext;

    public AppointmentTypeRepository(ApplicationDbContext dbContext) {
        _dbContext = dbContext;
    }

    public async Task<List<AppointmentTypeModel>> GetAllAsync(long tenantId) {
        return await _dbContext.AppointmentsTypes.Where(a => a.TenantId == tenantId).ToListAsync();
    }

    public async Task<AppointmentTypeModel> GetByIdAsync(long id, long tenantId) {
        return await _dbContext.AppointmentsTypes.Where(a => a.Id == id && a.TenantId == tenantId).FirstOrDefaultAsync();
    }

    public async Task AddAsync(AppointmentTypeModel appointmentType) {
        _dbContext.Add(appointmentType);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(AppointmentTypeModel appointmentType) {
        _dbContext.Entry(appointmentType).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id, long tenantId) {
        var appointmentType = await GetByIdAsync(id, tenantId);
        if (appointmentType != null) {
            _dbContext.Remove(appointmentType);
            await _dbContext.SaveChangesAsync();
        }
    }
}
