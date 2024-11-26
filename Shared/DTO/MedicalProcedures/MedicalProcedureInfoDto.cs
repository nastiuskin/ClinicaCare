using Domain.MedicalProcedures;
using System.Text.Json.Serialization;

namespace Application.MedicalProcedureManagement.DTO
{
    public class MedicalProcedureInfoDto
    {
        [JsonPropertyName("Id")]
        public Guid Id { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Type")]
        public string Type { get; set; }
        //public TimeSpan Duration { get; set; }
        //public decimal Price { get; set; }
    }
}
