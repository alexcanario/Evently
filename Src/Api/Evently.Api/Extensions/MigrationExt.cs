using Evently.Modules.Events.Infrastructure.Database;

using Microsoft.EntityFrameworkCore;

namespace Evently.Api.Extensions;

internal static class MigrationExt
{
    internal static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        ApplyMigration<EventsDbContext>(scope);
    }

    private static void ApplyMigration<TDbContext>(IServiceScope scope) where TDbContext : DbContext
    {
        ILogger logger = scope.ServiceProvider.GetRequiredService<ILogger<TDbContext>>();
        try
        {
            using TDbContext context = scope.ServiceProvider.GetRequiredService<TDbContext>();
            context.Database.Migrate();
            logger.LogInformation("Applied migrations for {DbContext}", typeof(TDbContext).Name);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while applying migrations for {DbContext}", typeof(TDbContext).Name);
            throw new InvalidOperationException($"Failed to apply migrations for {typeof(TDbContext).Name}", ex);
        }
    }
}
