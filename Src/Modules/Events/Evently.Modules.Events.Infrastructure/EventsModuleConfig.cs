using Evently.Modules.Events.Application.Abstraction.Data;
using Evently.Modules.Events.Domain.Events.Abstractions;
using Evently.Modules.Events.Infrastructure.Events;
using Evently.Modules.Events.Presentation.Events;

using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Npgsql;

namespace Evently.Modules.Events.Infrastructure;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Major Code Smell",
    "S2325:Methods and properties that don’t access instance data should be static",
    Justification = "False positive for C# 14 extension members")]

public static class EventsModuleConfig
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        EventEndpoints.MapEndpoints(app);
    }

    extension(IServiceCollection services)
    {
        public IServiceCollection AddEventsModule(IConfiguration configuration)
        {
            services.AddMediatR(config =>
                config.RegisterServicesFromAssemblies(Application.AssemblyReference.Assembly));

            services.AddInfrastructure(configuration);

            return services;
        }

        private void AddInfrastructure(IConfiguration configuration)
        {
            string databaseConnectionString = configuration.GetConnectionString("EventsDatabase")
                                              ?? throw new InvalidOperationException("Events database connection string is not configured.");

            NpgsqlDataSource npgsqlDataSource = new NpgsqlDataSourceBuilder(databaseConnectionString).Build();

            services.TryAddSingleton(npgsqlDataSource);

            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

            services.AddDbContext<EventsDbContext>(options =>
                options.UseNpgsql(
                        databaseConnectionString,
                        npgsqlOptions => npgsqlOptions
                            .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Events))
                    .UseSnakeCaseNamingConvention());

            services.AddLocalization();

            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<EventsDbContext>());
        }
    }
}
