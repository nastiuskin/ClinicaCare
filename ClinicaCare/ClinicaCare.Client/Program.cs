using Blazored.LocalStorage;
using ClinicaCare.Client.Services;
using ClinicaCare.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Blazr.RenderState.WASM;
using ClinicaCare.Client.Services.Auth;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();

builder.Services.AddTransient<TokenHandler>();

builder.Services.AddScoped(sp =>
{
    var handler = sp.GetRequiredService<TokenHandler>();

    return new HttpClient(handler)
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    };
});

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.AddBlazrRenderStateWASMServices();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMedicalProcedureService, MedicalProcedureService>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddMudServices();

builder.Services.AddCascadingAuthenticationState();
await builder.Build().RunAsync();
