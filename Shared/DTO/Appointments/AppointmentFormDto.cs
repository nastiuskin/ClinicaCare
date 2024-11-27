using System.ComponentModel.DataAnnotations;

namespace Application.AppointmentManagement.DTO
{
    public class AppointmentFormDto
    {
        [Required(ErrorMessage = "Please select a doctor")]
        public Guid DoctorId { get; set; }

        public Guid PatientId { get; set; }

        [Required(ErrorMessage="Please select a procedure")]
        public Guid MedicalProcedureId { get; set; }

        [Required(ErrorMessage = "Please select a date")]
        public string Date { get; set; }

        [Required(ErrorMessage = "Please select start time")]
        public string StartTime { get; set; }

        [Required(ErrorMessage = "Please select end time")]
        public string EndTime { get; set; }
    }
}
