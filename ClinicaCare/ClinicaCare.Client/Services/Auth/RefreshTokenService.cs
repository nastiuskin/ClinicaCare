using ClinicaCare.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace ClinicaCare.Client.Services.Auth
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        public RefreshTokenService(IHttpClientFactory httpClientFactory, ITokenService tokenService)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _tokenService = tokenService;
        }
        public async Task<string?> RefreshTokenAsync()
        {
            var response = await _httpClient.PostAsync("api/account/refresh", null);

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

