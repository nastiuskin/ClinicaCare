using Domain.Intefraces;
using Domain.MedicalProcedures;
using Domain.Users;

namespace Domain.Appointments
{
    public interface IAppointmentRepository : IBaseRepository<Appointment>
    {
        public Task<Appointment?> GetByIdAsync(AppointmentId id);
        public IQueryable<Appointment> GetAllByDoctorIdAsync(UserId doctorId, int pageNumber, int pageSize); //can filter by date/status
        public IQueryable<Appointment> GetAllByPatientIdAsync(UserId patientId, int pageNumber, int pageSize); //can filter by date/status
        public IQueryable<Appointment> GetAllByMedicalProcedureIdAsync(MedicalProcedureId mpId, int pageNumber, int pageSize); 
    }
}
