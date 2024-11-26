using Application.UserAccountManagement.Doctors.DTO;
using Domain.MedicalProcedures;
using Domain.Users.Doctors;
using System.Text.Json.Serialization;

namespace Application.MedicalProcedureManagement.DTO
{
    public class MedicalProcedureInfoWithDoctorsDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("duration")]
        public string Duration { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("doctors")]
        public List<DoctorPartialInfoDto> Doctors { get; set; }
    }
}
