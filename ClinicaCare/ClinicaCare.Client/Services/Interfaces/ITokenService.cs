using System.Security.Claims;

namespace ClinicaCare.Client.Services.Interfaces
{
    public interface ITokenService
    {
        public Task<string?> GetTokenAsync();
        public Task SetTokenAsync(string token);
        public Task RemoveTokenAsync();
        public bool IsTokenExpired(string token);
        public IEnumerable<Claim> ParseClaimsFromJwt(string jwt);
    }
}
