using ClinicaCare.Client.Services.Auth;
using ClinicaCare.Client.Services.Interfaces;
using System.Net.Http.Headers;

namespace ClinicaCare.Client.Services.Middlewares
{
    public class AddAuthorizationHeaderHandler(ITokenService tokenService) : DelegatingHandler
    {

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Started AuthorizationHeaderHandler");
            var token = await tokenService.GetTokenAsync();

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
             return await base.SendAsync(request, cancellationToken);
        }
    }
}
