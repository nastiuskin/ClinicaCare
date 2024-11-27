﻿using Application.MedicalProcedureManagement.DTO;
using Application.UserAccountManagement.Doctors.DTO;
using Application.UserAccountManagement.UserDtos;
using ClinicaCare.Client.Services.Interfaces;
using ClinicaCare.Client.Services.Pagination;
using Domain.Helpers.PaginationStuff;
using Shared.DTO.Users;
using System.Net.Http.Json;
using System.Text.Json;

namespace ClinicaCare.Client.Services
{
    public class UserService(IHttpClientFactory httpClientFactory) : IUserService
    {
        public async Task<(UserViewDto User, string ErrorMessage)> GetProfile()
        {
            try
            {
                using HttpClient _httpClient = httpClientFactory.CreateClient("ApiClient");
                var response = await _httpClient.GetAsync("api/account/profile");

                if (response.IsSuccessStatusCode)
                {
                    var userProfile = await response.Content.ReadFromJsonAsync<UserViewDto>();
                    return (userProfile, string.Empty);
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    var apiErrors = JsonSerializer.Deserialize<List<ApiErrorResponse>>(errorResponse);

                    var errorMessage = apiErrors?.FirstOrDefault()?.Message ?? "An unknown error occurred.";
                    return (null, errorMessage);
                }
            }
            catch (Exception ex)
            {
                return (null, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<(bool Success, string[] Errors)> UpdateAsync(UserViewDto userViewDto)
        {
            try
            {
                using HttpClient _httpClient = httpClientFactory.CreateClient("ApiClient");
                var response = await _httpClient.PutAsJsonAsync("api/account/profile/edit", userViewDto);

                if (response.IsSuccessStatusCode)
                {
                    return (true, Array.Empty<string>());
                }

                var errorResponse = await response.Content.ReadAsStringAsync();

                try
                {
                    var apiErrors = JsonSerializer.Deserialize<List<ApiErrorResponse>>(errorResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    var errorMessages = apiErrors?.Select(e => e.Message).ToArray() ?? new[] { "An unknown error occurred." };
                    return (false, errorMessages);
                }
                catch (JsonException)
                {
                    return (false, new[] { "Failed to parse server response."});
                }
            }
            catch (Exception ex)
            {
                return (false, new[] { $"An error occurred: {ex.Message}" });
            }
        }

        public async Task<PagingResponse<DoctorPartialInfoDto>> GetPagiantedDoctorsAsync(DoctorParameters parameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = parameters.PageNumber.ToString()
            };

            var queryString = string.Join("&", queryStringParam.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            using HttpClient _httpClient = httpClientFactory.CreateClient("ApiClient");
            var response = await _httpClient.GetAsync($"api/account/doctors?{queryString}");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var items = JsonSerializer.Deserialize<List<DoctorPartialInfoDto>>(content);
            var metaData = JsonSerializer.Deserialize<MetaData>(response.Headers.GetValues("X-Pagination").First());

            var pagingResponse = new PagingResponse<DoctorPartialInfoDto>
            {
                Items = items,
                MetaData = metaData
            };
            return pagingResponse;
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            try
            {
                using HttpClient _httpClient = httpClientFactory.CreateClient("ApiClient");
                var response = await _httpClient.DeleteAsync($"api/admin/users/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    throw new ApplicationException($"Failed to delete user. Server responded with: {content}");
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<(bool Success, List<DoctorPartialInfoDto>)> GetAllDoctorsAsync()
        {
            try
            {
                using HttpClient _httpClient = httpClientFactory.CreateClient("ApiClient");
                var response = await _httpClient.GetAsync("api/account/doctors-list");

                if (!response.IsSuccessStatusCode)
                {
                    return (false, null);
                }

                var content = await response.Content.ReadAsStringAsync();
                var doctors = JsonSerializer.Deserialize<List<DoctorPartialInfoDto>>(content);
                return (true, doctors);
            }
            catch (Exception ex)
            {
                return (false, null);
            }
        }

        public async Task<bool> CreateDoctorAsync(DoctorFormDto doctorFormDto)
        {
            try
            {
                using HttpClient _httpClient = httpClientFactory.CreateClient("ApiClient");
                var response = await _httpClient.PostAsJsonAsync("api/admin/doctors", doctorFormDto);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<(bool, DoctorViewDto)> GetDoctorByIdAsync(Guid id)
        {
            try
            {
                using HttpClient _httpClient = httpClientFactory.CreateClient("ApiClient");
                var response = await _httpClient.GetAsync($"api/account/{id}/doctor-profile");

                if (response.IsSuccessStatusCode)
                {
                    var doctorProfile = await response.Content.ReadFromJsonAsync<DoctorViewDto>();
                    return (true, doctorProfile);
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    var apiErrors = JsonSerializer.Deserialize<List<ApiErrorResponse>>(errorResponse);

                    var errorMessage = apiErrors?.FirstOrDefault()?.Message ?? "An unknown error occurred.";
                    return (false, null);
                }
            }
            catch (Exception ex)
            {
                return (false, null);
            }
        }

    }
}


