using API.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureServices();
builder.Services.InitializeRepositories();

builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);

builder.Services.InitializeControllers();

builder.Services.ConfigureSwagger();

var app = builder.Build();

await app.SeedDataAsync();

app.ConfigureMiddleware();

app.Run();
