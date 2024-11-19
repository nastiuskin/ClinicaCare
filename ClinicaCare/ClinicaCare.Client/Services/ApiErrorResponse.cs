using System.Text.Json.Serialization;

namespace ClinicaCare.Client.Services
{
    public class ApiErrorResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("reasons")]
        public List<string> Reasons { get; set; }

        [JsonPropertyName("metadata")]
        public Dictionary<string, object> Metadata { get; set; }
    }
}
