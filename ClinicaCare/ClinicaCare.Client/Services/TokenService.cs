using Blazored.LocalStorage;
using ClinicaCare.Client.Services.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Text.Json;

namespace ClinicaCare.Client.Services
{
    public class TokenService : ITokenService
    {
        private readonly ILocalStorageService _localStorage;

        public TokenService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<string?> GetTokenAsync()
        {
            return await _localStorage.GetItemAsync<string>("accessToken");
        }

        public async Task SetTokenAsync(string token)
        {
            await _localStorage.SetItemAsync("accessToken", token);
        }

        public async Task RemoveTokenAsync()
        {
            await _localStorage.RemoveItemAsync("accessToken");
        }


        public bool IsTokenExpired(string token)
        {
            var expirationClaim = ParseClaimsFromJwt(token).FirstOrDefault(c => c.Type == "exp");
            if (expirationClaim == null)
                return false;

            var expirationDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expirationClaim.Value));
            return expirationDate <= DateTimeOffset.UtcNow;
        }

        public IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = WebEncoders.Base64UrlDecode(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }


    }

}
