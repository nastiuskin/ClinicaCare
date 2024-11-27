using Application.Helpers.PaginationStuff;
using Domain.Helpers.PaginationStuff;
using Domain.Intefraces;
using Domain.MedicalProcedures;
using Domain.Users;

namespace Domain.Appointments
{
    public interface IAppointmentRepository : IBaseRepository<Appointment>
    {
        public Task<Appointment?> GetByIdAsync(AppointmentId id);
        public IQueryable<Appointment> GetAllAppointmentsByDoctorIdAsync(UserId doctorId, int pageNumber, int pageSize); //can filter by date/status
        public IQueryable<Appointment> GetAllAppointmentsByPatientIdAsync(UserId patientId, int pageNumber, int pageSize); //can filter by date/status
        public IQueryable<Appointment> GetAllAppointmentsByMedicalProcedureIdAsync(MedicalProcedureId mpId, int pageNumber, int pageSize);

        public PagedList<Appointment> GetAppointmentsAsync(AppointmentParameters parameters,string roleClaim, UserId userId);

    }
}
