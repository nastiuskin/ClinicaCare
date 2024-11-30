using Blazored.LocalStorage;
using ClinicaCare.Client.Services.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace ClinicaCare.Client.Services.Auth
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

            var claims = new List<Claim>();

            if (keyValuePairs == null)
                return claims;

            foreach (var kvp in keyValuePairs)
            {
                if (kvp.Key == "sub" && kvp.Value is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Object)
                {
                    var subDict = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonElement.GetRawText());
                    if (subDict != null && subDict.TryGetValue("Value", out var subValue))
                    {
                        claims.Add(new Claim("sub", subValue));
                    }
                }
                else if (kvp.Key == "role")
                {
                    if (kvp.Value is JsonElement roleElement && roleElement.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var role in roleElement.EnumerateArray())
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role.GetString()));
                        }
                    }
                    else
                    {
                        claims.Add(new Claim(ClaimTypes.Role, kvp.Value.ToString()));
                    }
                }
                else
                {
                    claims.Add(new Claim(kvp.Key, kvp.Value.ToString()));
                }
            }

            return claims;
        }

    }
}
