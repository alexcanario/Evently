using Evently.Modules.Events.Api.Database;
using Evently.Modules.Events.Api.Events;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Modules.Events.Api;

public static class EventsModuleConfig
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        CreateEvent.MapEndpoint(app);
        GetEvent.MapEndpoint(app);
    }

    public static IServiceCollection AddEventsModule(this IServiceCollection services, IConfiguration configuration)
    {
        string databaseConnectionString = configuration.GetConnectionString("EventsDatabase") 
            ?? throw new InvalidOperationException("Events database connection string is not configured.");

        services.AddDbContext<EventsDbContext>(options =>
            options.UseNpgsql(
                databaseConnectionString, 
                npgsqlOptions => npgsqlOptions
                    .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Events))
                .UseSnakeCaseNamingConvention());

        services.AddLocalization();

        return services;
    }
}
