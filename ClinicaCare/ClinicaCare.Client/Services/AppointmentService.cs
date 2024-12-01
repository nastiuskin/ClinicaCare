using Application.AppointmentManagement.DTO;
using Application.MedicalProcedureManagement.DTO;
using ClinicaCare.Client.Services.Interfaces;
using ClinicaCare.Client.Services.Pagination;
using Domain.Appointments;
using Domain.Helpers.PaginationStuff;
using System.Net.Http.Json;
using System.Text.Json;

namespace ClinicaCare.Client.Services
{
    public class AppointmentService(IHttpClientFactory httpClientFactory) : IAppointmentService
    {
        public async Task<PagingResponse<AppointmentInfoDto>> GetAllAppontmentsAsync(AppointmentParameters parameters)
        {
            using HttpClient _httpClient = httpClientFactory.CreateClient("ApiClient");
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = parameters.PageNumber.ToString(),
            };
            var queryString = string.Join("&", queryStringParam.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            var response = await _httpClient.GetAsync($"api/appointments?{queryString}");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var items = JsonSerializer.Deserialize<List<AppointmentInfoDto>>(content);
            var metaData = JsonSerializer.Deserialize<MetaData>(response.Headers.GetValues("X-Pagination").First());

            var pagingResponse = new PagingResponse<AppointmentInfoDto>
            {
                Items = items,
                MetaData = metaData
            };

            return pagingResponse;
        }


        public async Task<List<TimeSlotDto>> GetAvailableTimeSlots(Guid doctorId, Guid medicalProcedureId, string date)
        {
            using HttpClient httpClient = httpClientFactory.CreateClient("ApiClient");

            var queryStringParam = new Dictionary<string, string>
            {
                ["doctorId"] = doctorId.ToString(),
                ["medicalProcedureId"] = medicalProcedureId.ToString(),
                ["date"] = date
            };

            var queryString = string.Join("&", queryStringParam.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            var response = await httpClient.GetAsync($"api/appointments/available-time-slots?{queryString}");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var timeSlots = JsonSerializer.Deserialize<List<TimeSlotDto>>(content);
            return timeSlots;
        }

        public async Task<bool> CreateAppointment(AppointmentFormDto appointmentFormDto)
        {
            using HttpClient httpClient = httpClientFactory.CreateClient("ApiClient");
            var response = await httpClient.PostAsJsonAsync("api/appointments", appointmentFormDto);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        public async Task<string> AddFeedback(Guid appointmentId, string feedback)
        {
            using HttpClient httpClient = httpClientFactory.CreateClient("ApiClient");

            if (string.IsNullOrEmpty(feedback))
            {
                return "Feedback cannot be empty.";
            }

            try
            {
                var response = await httpClient.PostAsJsonAsync($"api/appointments/{appointmentId}/feedback", feedback);

                if (response.IsSuccessStatusCode)
                {
                    return "Feedback added successfully!";
                }

                return $"Failed to submit feedback. Status code: {response.StatusCode}";
            }
            catch (Exception)
            {
                return "An error occurred while submitting your feedback.";
            }
        }

        public async Task<bool> EditAppointmentStatus(Guid appointmentId, AppointmentStatus status)
        {
            using HttpClient httpClient = httpClientFactory.CreateClient("ApiClient");

            HttpResponseMessage response;

            if (status.Equals(AppointmentStatus.CANCELED))
            {
                response = await httpClient.PostAsJsonAsync($"api/appointments/{appointmentId}/cancel", status);
            }
            else
            {
                response = await httpClient.PostAsJsonAsync($"api/appointments/{appointmentId}/complete", status);
            }

            if (response.IsSuccessStatusCode)
                return true;
            else return false;
        }

    }

}
