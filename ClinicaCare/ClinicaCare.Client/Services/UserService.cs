using Application.UserAccountManagement.UserDtos;
using ClinicaCare.Client.Services.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace ClinicaCare.Client.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(UserViewDto User, string ErrorMessage)> GetProfile()
        {
            try
            {
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

    }
}
