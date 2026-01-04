using Evently.Api.Config;
using Evently.Api.Extensions;
using Evently.Modules.Events.Infrastructure;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configuração para IIS
builder.WebHost.UseIISIntegration();

builder.Services.AddOpenApi();

builder.Services.AddCultures();

builder.Services.AddEventsModule(builder.Configuration);

WebApplication app = builder.Build();

// Usar Forwarded Headers (importante para IIS)
app.UseForwardedHeaders();

app.UseRequestLocalization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.ApplyMigrations();
    app.MapScalarApiReference();
}

EventsModuleConfig.MapEndpoints(app);

await app.RunAsync();
