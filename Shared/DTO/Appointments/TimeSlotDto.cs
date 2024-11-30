using System.Text.Json.Serialization;

namespace Application.AppointmentManagement.DTO
{
    public class TimeSlotDto
    {
        [JsonPropertyName("startTime")]
        public string StartTime { get; set; }

        [JsonPropertyName("endTime")]
        public string EndTime { get; set; }

        public override string ToString()
        {
            return $"{StartTime} - {EndTime}";
        }
    }
}
