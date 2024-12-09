using API.Extensions;
using Blazr.RenderState.Server;
using ClinicaCare.Client.Services;
using ClinicaCare.Components;
using ClinicaCare.Hubs;
using ClinicaCare.SignalR;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.ResponseCompression;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

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

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<INotificationService, NotificationService>();

var app = builder.Build();

app.UseResponseCompression();

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

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ClinicaCare.Client._Imports).Assembly);

app.UseCors("CorsPolicy");
app.MapHub<NotificationHub>("/notificationHub");
app.Run();
