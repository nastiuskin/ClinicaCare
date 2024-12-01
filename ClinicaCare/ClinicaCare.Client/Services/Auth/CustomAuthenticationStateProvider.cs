using ClinicaCare.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ClinicaCare.Client.Services.Auth
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ITokenService _tokenService;

        public CustomAuthenticationStateProvider(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _tokenService.GetTokenAsync();

            var identity = new ClaimsIdentity();

            if (!string.IsNullOrEmpty(token))
            {
                var claims = _tokenService.ParseClaimsFromJwt(token);
                foreach (var claim in claims)
                {
                    Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
                }
                identity = new ClaimsIdentity(claims, "jwt");
            }

            var user = new ClaimsPrincipal(identity);


            return new AuthenticationState(user);
        }
    }
}
