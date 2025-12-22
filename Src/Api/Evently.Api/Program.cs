using Evently.Api.Config;
using Evently.Api.Extensions;
using Evently.Modules.Events.Api;
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

// Em produção, também expor OpenAPI e Scalar para facilitar testes
if (app.Environment.IsProduction())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    
    // Aplicar migrações em produção (APENAS em IIS local/desenvolvimento)
    // REMOVA esta linha em ambiente de produção real!
    // Use dotnet ef database update ou scripts SQL em produção
    bool applyMigrationsInProduction = builder.Configuration.GetValue<bool>("ApplyMigrationsOnStartup", false);
    if (applyMigrationsInProduction)
    {
        app.ApplyMigrations();
    }
}

EventsModuleConfig.MapEndpoints(app);

await app.RunAsync();
