using ClinicaCare.Client.Services.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;

namespace ClinicaCare.Client.Services.Auth
{
    public class RefreshTokenService(IHttpClientFactory httpClientFactory, ITokenService _tokenService) : IRefreshTokenService
    {
        public async Task<bool> RefreshTokenAsync()
        {
            using HttpClient client = httpClientFactory.CreateClient("RefreshClient");
            var response = await client.PostAsync("api/account/refresh", null);

            if (response.IsSuccessStatusCode)
            {
                var newAccessToken = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(newAccessToken))
                {
                    await _tokenService.SetTokenAsync(newAccessToken);
                    return true;
                }
            }
            return false;
        }
    }
}

