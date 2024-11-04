namespace Application.AppointmentManagement.DTO
{
    public class AppointmentFormDto
    {
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public Guid MedicalProcedureId { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
