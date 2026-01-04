using System.Data.Common;

using Dapper;

using Evently.Modules.Events.Application.Abstraction.Data;
using Evently.Modules.Events.Domain.Events.Abstractions;
using Evently.Modules.Events.Domain.Events.Models;

namespace Evently.Modules.Events.Infrastructure.Events;

internal sealed class EventRepository(EventsDbContext context, IDbConnectionFactory dbConnectionFactory)
    : IEventRepository
{
    public void Insert(Event @event)
    {
        context.Add(@event);
    }

    public async Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using DbConnection dbConnection = await dbConnectionFactory.OpenConnectionAsync(cancellationToken);

        const string sql =
            """
                SELECT 
                    id AS Id,
                    title AS Title,
                    description AS Description,
                    location AS Location,
                    starts_at_utc AS StartsAtUtc,
                    ends_at_utc AS EndsAtUtc,
                    status AS Status
                FROM events.events 
                WHERE id = @Id
            """;

        return await dbConnection.QuerySingleOrDefaultAsync<Event>(sql, new { Id = id });
    }
}
