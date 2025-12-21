using Evently.Modules.Events.Api;
using System.Globalization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddEventsModule(builder.Configuration);

builder.Services.AddLocalization();

builder.Services.AddEventsModule(builder.Configuration);

WebApplication app = builder.Build();

app.UseRequestLocalization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

Evently.Modules.Events.Api.EventsModuleConfig.MapEndpoints(app);

await app.RunAsync();
