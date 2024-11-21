using API.Extensions;
using Blazored.LocalStorage;
using Blazr.RenderState.Server;
using ClinicaCare.Client.Services;
using ClinicaCare.Client.Services.Interfaces;
using ClinicaCare.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();
builder.AddBlazrRenderStateServerServices();
builder.Services.AddMudServices();
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureServices();
builder.Services.InitializeRepositories();

builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);

builder.Services.InitializeControllers();

builder.Services.ConfigureSwagger();

builder.Services.AddHttpClient();

builder.Services.AddAuthorizationCore();


var app = builder.Build();

await app.SeedDataAsync();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.ConfigureMiddleware();
app.UseAntiforgery();
app.MapControllers();


app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ClinicaCare.Client._Imports).Assembly);

app.Run();
