using Application.UserAccountManagement.UserDtos;
using Blazored.LocalStorage;
using ClinicaCare.Client.Services.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace ClinicaCare.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        public AuthService(HttpClient client, ITokenService tokenService)
        {
            _httpClient = client;
            _tokenService = tokenService;
        }

        public async Task<(bool Success, string[] Errors)> LoginAsync(UserLoginDto userLoginDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/account/login", userLoginDto);

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
            catch (Exception ex)
            {
                return (false, new[] { $"An error occurred: {ex.Message}" });
            }
        }


        public async Task<(bool Success, string[] Errors)> RegisterAsync(UserFormDto userFormDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/account/register", userFormDto);

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
            catch (Exception ex)
            {
                return (false, new[] { $"An error occurred: {ex.Message}" });
            }
        }

        public async Task<(bool Success, string[] Errors)> LogoutAsync()
        {
            try
            {
                var response = await _httpClient.PostAsync("/api/account/logout", null);

                if (response.IsSuccessStatusCode)
                {
                    await _tokenService.RemoveTokenAsync();
                    return (true, Array.Empty<string>());
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    return (false, new[] { $"Logout failed: {errorResponse}" });
                }
            }
            catch (Exception ex)
            {
                return (false, new[] { $"An error occurred: {ex.Message}" });
            }
        } 

        public async Task<(bool Success, string? Token, string? ErrorMessage)> RefreshTokenAsync()
        {
            try
            {
                var response = await _httpClient.PostAsync("/refresh", null);

                if (response.IsSuccessStatusCode)
                {
                    var newToken = await response.Content.ReadFromJsonAsync<string>();
                    if (!string.IsNullOrEmpty(newToken))
                    {
                        await _tokenService.SetTokenAsync(newToken);
                        return (true, newToken, null);
                    }

                    return (false, null, "Received an invalid token response.");
                }

                var errorMessage = await response.Content.ReadAsStringAsync();
                return (false, null, $"Token refresh failed with error: {errorMessage}");
            }
            catch (Exception ex)
            {
                return (false, null, $"An exception occurred while refreshing the token: {ex.Message}");
            }
        }


    }
}

