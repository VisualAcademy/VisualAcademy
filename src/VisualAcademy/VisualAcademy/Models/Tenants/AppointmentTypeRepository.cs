using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VisualAcademy.Data;
using VisualAcademy.Models.Tenants;

namespace VisualAcademy.Repositories.Tenants {
    public class AppointmentTypeRepository : IAppointmentTypeRepository {
        private readonly ApplicationDbContext _context;

        public AppointmentTypeRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<List<AppointmentTypeModel>> GetAllAsync() {
            return await _context.AppointmentsTypes.ToListAsync();
        }

        public async Task<AppointmentTypeModel> GetByIdAsync(long id) {
            return await _context.AppointmentsTypes.FindAsync(id);
        }

        public async Task CreateAsync(AppointmentTypeModel appointmentType) {
            _context.Add(appointmentType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AppointmentTypeModel appointmentType) {
            _context.Update(appointmentType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id) {
            var appointmentType = await _context.AppointmentsTypes.FindAsync(id);
            _context.Remove(appointmentType);
            await _context.SaveChangesAsync();
        }
    }
}
