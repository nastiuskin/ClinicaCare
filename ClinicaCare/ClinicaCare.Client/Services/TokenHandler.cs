using Blazored.LocalStorage;
using ClinicaCare.Client.Services.Interfaces;
using System.Net.Http.Headers;

namespace ClinicaCare.Client.Services
{
    public class TokenHandler : DelegatingHandler   
    {
        private readonly ITokenService _tokenService;
        //private readonly IRefreshTokenService _refreshTokenService;
        private readonly ILocalStorageService _localStorageService;

        public TokenHandler(ILocalStorageService localStorage, ITokenService tokenService) : base(new HttpClientHandler())
        {
            _localStorageService = localStorage;
            _tokenService = tokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Started");
            var token = await _tokenService.GetTokenAsync();
            //var token = await _localStorageService.GetItemAsync<string>("accessToken");

            //if (!string.IsNullOrEmpty(token))
            //{
            //    if (_tokenService.IsTokenExpired(token))
            //    {
            //        try
            //        {
            //            token = await _refreshTokenService.RefreshTokenAsync();
            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine($"Token refresh failed: {ex.Message}");
            //        }
            //    }

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }



        //protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        //{
        //    var token = await _tokenService.GetTokenAsync();
        //    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    var response = await base.SendAsync(request, cancellationToken);

        //    if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
        //    {
        //        token = await _refreshTokenService.RefreshTokenAsync();
        //        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //        response = await base.SendAsync(request, cancellationToken);
        //    }

        //    return response;
        //}

    }
}

