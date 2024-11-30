using Application.UserAccountManagement.UserDtos;
using Blazored.LocalStorage;
using ClinicaCare.Client.Services.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace ClinicaCare.Client.Services.Auth
{
    public class AuthService(IHttpClientFactory _httpClientFactory, ITokenService _tokenService) : IAuthService
    {
        public async Task<(bool Success, string[] Errors)> LoginAsync(UserLoginDto userLoginDto)
        {
            using HttpClient client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PostAsJsonAsync("api/account/login", userLoginDto);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                await _tokenService.SetTokenAsync(token);

                return (true, Array.Empty<string>());
            }

            var errorResponse = await response.Content.ReadAsStringAsync();
            var apiErrors = JsonSerializer.Deserialize<List<ApiErrorResponse>>(errorResponse);

            var errorMessage = apiErrors?.FirstOrDefault()?.Message ?? "An unknown error occurred.";
            return (false, new[] { errorMessage });
        }


        public async Task<(bool Success, string[] Errors)> RegisterAsync(UserFormDto userFormDto)
        {
            using HttpClient client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PostAsJsonAsync("/api/account/register", userFormDto);

            if (response.IsSuccessStatusCode)
            {
                return (true, Array.Empty<string>());
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                return (false, new[] { $"Registration failed: {errorResponse}" });
            }
        }

        public async Task<bool> LogoutAsync()
        {
            using HttpClient client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PostAsync("/api/account/logout", null);

            if (response.IsSuccessStatusCode)
            {
                await _tokenService.RemoveTokenAsync();
                return true;
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                return false;
            }
        }
    }
}



