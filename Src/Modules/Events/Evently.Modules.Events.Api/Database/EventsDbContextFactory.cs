using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Evently.Modules.Events.Api.Database;

/// <summary>
/// Factory para criar instâncias do EventsDbContext em tempo de design.
/// Utilizada pelas ferramentas do Entity Framework Core (migrations, etc.)
/// </summary>
public sealed class EventsDbContextFactory : IDesignTimeDbContextFactory<EventsDbContext>
{
    public EventsDbContext CreateDbContext(string[] args)
    {
        // Obtém a connection string de variável de ambiente ou usa valor padrão
        string connectionString = Environment.GetEnvironmentVariable("EVENTS_DB_CONNECTION_STRING")
            ?? "Host=localhost;Port=5432;Database=evently;Username=postgres;Password=postgres;Include Error Detail=true";

        DbContextOptionsBuilder<EventsDbContext> optionsBuilder = new();
        
        optionsBuilder.UseNpgsql(
                connectionString,
                npgsqlOptions => npgsqlOptions
                    .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Events))
            .UseSnakeCaseNamingConvention();

        return new EventsDbContext(optionsBuilder.Options);
    }
}
