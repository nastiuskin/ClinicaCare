using ClinicaCare.Client.Services.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;

namespace ClinicaCare.Client.Services.Auth
{
    public class RefreshTokenService(IHttpClientFactory httpClientFactory, ITokenService _tokenService) : IRefreshTokenService
    {
        public async Task<string?> RefreshTokenAsync()
        {
            using HttpClient client = httpClientFactory.CreateClient("TokenClient");
            var response = await client.PostAsync("api/account/refresh", null);

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

