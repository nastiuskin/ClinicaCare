using Application.Helpers.PaginationStuff;
using Domain.Appointments;
using Domain.Helpers.PaginationStuff;
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
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task DeleteAsync(Appointment entity)
        {
            _context.Appointments.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Appointment> GetAll()
        {
            return _context.Appointments;
                
        }

        public async Task UpdateAsync(Appointment entity)
        {
            _context.Appointments.Update(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Appointment> GetAllAppointmentsByDoctorIdAsync(UserId doctorId, int pageNumber, int pageSize)
        {
            return _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize); 
        }

        public IQueryable<Appointment> GetAllAppointmentsByPatientIdAsync(UserId patientId, int pageNumber, int pageSize)
        {
            return _context.Appointments
                .Where(a => a.PatientId == patientId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize); 
        }

        public IQueryable<Appointment> GetAllAppointmentsByMedicalProcedureIdAsync(MedicalProcedureId id, int pageNumber, int pageSize)
        {
            return _context.Appointments
                .Where(a => a.MedicalProcedureId == id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Appointments.CountAsync();
        }

        public PagedList<Appointment> GetAppointmentsAsync(AppointmentParameters parameters, string roleClaim, UserId userId)
        {
            var query = GetAll();

            if (roleClaim == "Doctor")
            {
                query = query.Where(a => a.DoctorId == userId);
            }
            else
            {
                query = query.Where(a => a.PatientId == userId);
            }

            return PagedList<Appointment>
                .ToPagedList(query, parameters.PageNumber, parameters.PageSize);
        }
    }

    }

