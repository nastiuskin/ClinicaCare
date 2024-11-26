using System.Text.Json.Serialization;

namespace Application.UserAccountManagement.Doctors.DTO
{
    public class DoctorPartialInfoDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("specializationType")]
        public string SpecializationType { get; set; }
    }
}
