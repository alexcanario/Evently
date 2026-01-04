using Evently.Modules.Events.Application.Abstraction.Data;
using Evently.Modules.Events.Domain.Events.Models;

namespace Evently.Modules.Events.Infrastructure.Database;

public sealed class EventsDbContext(DbContextOptions<EventsDbContext> options) 
    : DbContext(options), IUnitOfWork
{
    internal DbSet<Event> Events => Set<Event>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Events);
    }
}
