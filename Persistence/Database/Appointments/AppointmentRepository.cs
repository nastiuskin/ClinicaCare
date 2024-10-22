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

        public async Task DeleteAsync(Appointment entity)
        {
           _context.Appointments.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
           return await _context.Appointments.ToListAsync();
        }

        public async Task UpdateAsync(Appointment entity)
        {
            _context.Appointments.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAllByDoctorIdAsync(UserId doctorId)
        {
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAllByPatientIdAsync(UserId patientId)
        {
            return await _context.Appointments
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAllByMedicalProcedureIdAsync(MedicalProcedureId id)
        {
            return await _context.Appointments
                .Where(a => a.MedicalProcedureId == id)
                .ToListAsync();
        }
    }
}
