using Domain.Appointments;

namespace Application.AppointmentManagement.DTO
{
    public class AppointmentInfoDto
    {
        public Guid Id { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public string MedicalProcedureName { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Status { get; set; }
        public string? FeedBack { get; set; }
    }
}
