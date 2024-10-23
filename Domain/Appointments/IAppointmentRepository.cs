using Domain.Intefraces;
using Domain.MedicalProcedures;
using Domain.Users;

namespace Domain.Appointments
{
    public interface IAppointmentRepository : IBaseRepository<Appointment>
    {
        public Task<Appointment?> GetByIdAsync(AppointmentId id);
        public Task<IEnumerable<Appointment>> GetAllByDoctorIdAsync(UserId doctorId, int pageNumber, int pageSize);
        public Task<IEnumerable<Appointment>> GetAllByPatientIdAsync(UserId patientId, int pageNumber, int pageSize);
        public Task<IEnumerable<Appointment>> GetAllByMedicalProcedureIdAsync(MedicalProcedureId mpId, int pageNumber, int pageSize);
    }
}
