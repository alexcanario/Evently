using Evently.Api.Config;
using Evently.Api.Extensions;
using Evently.Modules.Events.Api;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddCultures();

builder.Services.AddEventsModule(builder.Configuration);

WebApplication app = builder.Build();

app.UseRequestLocalization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.ApplyMigrations();
    app.MapScalarApiReference();
}

EventsModuleConfig.MapEndpoints(app);

await app.RunAsync();
