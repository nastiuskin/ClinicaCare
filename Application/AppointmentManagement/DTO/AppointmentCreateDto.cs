using Domain.MedicalProcedures;
using Domain.Users;
using Domain.ValueObjects;

namespace Application.AppointmentManagement.DTO
{
    public class AppointmentCreateDto
    {
        public UserId DoctorId { get; private set; }
        public UserId PatientId { get; private set; }
        public MedicalProcedureId MedicalProcedureId { get; private set; }
        public DateOnly Date { get; private set; }
        public TimeSlot Duration { get; private set; }
    }
}
