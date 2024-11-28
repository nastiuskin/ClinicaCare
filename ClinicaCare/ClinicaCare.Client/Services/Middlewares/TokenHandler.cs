using Blazored.LocalStorage;
using ClinicaCare.Client.Services.Auth;
using ClinicaCare.Client.Services.Interfaces;
using System.Net;
using System.Net.Http.Headers;

namespace ClinicaCare.Client.Services.Middlewares
{
    public class TokenHandler(IRefreshTokenService refreshTokenService, ITokenService tokenService) : DelegatingHandler
    {

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Started TokenHandler");

            var token = await tokenService.GetTokenAsync();

            if (!string.IsNullOrEmpty(token))
            {
                if (tokenService.IsTokenExpired(token))
                {
                    try
                    {
                        var refreshResult = await refreshTokenService.RefreshTokenAsync();

                        if (!refreshResult)
                        {
                            return new HttpResponseMessage()
                            {
                                ReasonPhrase = "Failed to refresh token."
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        return new HttpResponseMessage()
                        {
                            ReasonPhrase = $"An error occurred while refreshing the token: {ex.Message}."
                        };
                    }
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
