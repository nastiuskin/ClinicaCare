using Application.AppointmentManagement.DTO;
using Application.Helpers.PaginationStuff;
using Application.MedicalProcedureManagement.DTO;
using ClinicaCare.Client.Services.Interfaces;
using ClinicaCare.Client.Services.Pagination;
using Domain.Appointments;
using Domain.Helpers.PaginationStuff;
using System.Net.Http;
using System.Text.Json;

namespace ClinicaCare.Client.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly HttpClient _httpClient;
        public AppointmentService(HttpClient httpClientFactory)
        {
            _httpClient = httpClientFactory;
        }
        public async Task<PagingResponse<AppointmentInfoDto>> GetAllAppontmentsAsync(AppointmentParameters parameters)
        {
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
    }
}
