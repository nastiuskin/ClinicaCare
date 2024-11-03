using Domain.MedicalProcedures;
using Domain.Users;
using Domain.ValueObjects;

namespace Application.AppointmentManagement.DTO
{
    public class AppointmentCreateDto
    {
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public Guid MedicalProcedureId { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
