using Domain.Appointments;
using System.Text.Json.Serialization;

namespace Application.AppointmentManagement.DTO
{
    public class AppointmentInfoDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("doctorName")]
        public string DoctorName { get; set; }

        [JsonPropertyName("patientName")]
        public string PatientName { get; set; }

        [JsonPropertyName("medicalProcedureName")]
        public string MedicalProcedureName { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("startTime")]
        public string StartTime { get; set; }

        [JsonPropertyName("endTime")]
        public string EndTime { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("feedback")]
        public string? FeedBack { get; set; }
    }
}
