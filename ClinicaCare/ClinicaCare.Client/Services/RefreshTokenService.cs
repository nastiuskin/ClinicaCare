using ClinicaCare.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace ClinicaCare.Client.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        public RefreshTokenService(HttpClient httpClient, ITokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }
        public async Task<string?> RefreshTokenAsync()
        {
            var response = await _httpClient.PostAsync("/refresh", null);

            if (response.IsSuccessStatusCode)
            {
                var newToken = await response.Content.ReadFromJsonAsync<string>();
                if (!string.IsNullOrEmpty(newToken))
                {
                    await _tokenService.SetTokenAsync(newToken);
                    return newToken;
                }
            }
            //needs refactoring
            return null;
        }
    }
}

