using Application.Helpers.PaginationStuff;
using Application.MedicalProcedureManagement.DTO;
using ClinicaCare.Client.Services.Interfaces;
using ClinicaCare.Client.Services.Pagination;
using Domain.Helpers.PaginationStuff;
using System.Net.Http.Json;
using System.Text.Json;

namespace ClinicaCare.Client.Services
{
    public class MedicalProcedureService : IMedicalProcedureService
    {
        private readonly HttpClient _httpClient;
        public MedicalProcedureService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<PagingResponse<MedicalProcedureInfoDto>> GetAllMedicalProceduresAsync(MedicalProcedureParameters parameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = parameters.PageNumber.ToString()
            };
            var queryString = string.Join("&", queryStringParam.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            var response = await _httpClient.GetAsync($"api/procedures?{queryString}");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var items = JsonSerializer.Deserialize<List<MedicalProcedureInfoDto>>(content);
            var metaData = JsonSerializer.Deserialize<MetaData>(response.Headers.GetValues("X-Pagination").First());

            var pagingResponse = new PagingResponse<MedicalProcedureInfoDto>
            {
                Items = items,
                MetaData = metaData
            };

            return pagingResponse;
        }

        public async Task<MedicalProcedureInfoWithDoctorsDto> GetMedicalProcedureAsync(Guid id)
        {
            var url = $"api/procedures/{id}";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    throw new ApplicationException($"Request failed: {content}");
                }

                var contentString = await response.Content.ReadAsStringAsync();
                var medicalProcedure = JsonSerializer.Deserialize<MedicalProcedureInfoWithDoctorsDto>(contentString);

                if (medicalProcedure == null)
                {
                    throw new ApplicationException("Failed to deserialize the response.");
                }

                return medicalProcedure;
            }
            catch (JsonException ex)
            {
                throw new ApplicationException("Error deserializing the response.", ex);
            }
        }

        public async Task<bool> DeleteMedicalProcedure(Guid id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/admin/procedures/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    throw new ApplicationException($"Failed to delete procedure. Server responded with: {content}");
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //needs error handling
        public async Task<bool> CreateMedicalProcedureAsync(MedicalProcedureFormDto medicalProcedureFormDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/admin/procedures", medicalProcedureFormDto);
                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateMedicalProcedureAsync(Guid id, MedicalProcedureUpdateDto medicalProcedureUpdateDto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/admin/procedures/{id}", medicalProcedureUpdateDto);
                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}