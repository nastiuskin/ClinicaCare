using Domain.Appointments;
using Domain.MedicalProcedures;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Database.Appointments
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;
        public AppointmentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Appointment entity)
        {
            _context.Appointments.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Appointment?> GetByIdAsync(AppointmentId id)
        {
            return await _context.Appointments
                .SingleOrDefaultAsync(a => a.Id == id);
        }

        //maybe to return the id???
        public async Task DeleteAsync(Appointment entity)
        {
            _context.Appointments.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _context.Appointments
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _context.Appointments
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .ToListAsync();
        }

        public async Task UpdateAsync(Appointment entity)
        {
            _context.Appointments.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAllByDoctorIdAsync(UserId doctorId, int pageNumber, int pageSize)
        {
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAllByPatientIdAsync(UserId patientId, int pageNumber, int pageSize)
        {
            return await _context.Appointments
                .Where(a => a.PatientId == patientId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAllByMedicalProcedureIdAsync(MedicalProcedureId id, int pageNumber, int pageSize)
        {
            return await _context.Appointments
                .Where(a => a.MedicalProcedureId == id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Appointments.CountAsync();
        }
    }
}
