using Blazored.LocalStorage;
using ClinicaCare.Client.Services.Interfaces;
using System.Net.Http.Headers;

namespace ClinicaCare.Client.Services.Auth
{
    public class TokenHandler : DelegatingHandler
    {
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenService _refreshTokenService;

        public TokenHandler(ITokenService tokenService, IRefreshTokenService refreshTokenService)
        {
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Started");
            var token = await _tokenService.GetTokenAsync();

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                if (_tokenService.IsTokenExpired(token))
                {
                    try
                    {
                        token = await _refreshTokenService.RefreshTokenAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Token refresh failed: {ex.Message}");
                    }
                }

            }

            return await base.SendAsync(request, cancellationToken);
        }

    }
}

