using System.Data.Common;

using Evently.Modules.Events.Application.Abstraction.Data;

using Npgsql;

namespace Evently.Modules.Events.Infrastructure.Database;

public sealed class DbConnectionFactory(NpgsqlDataSource datasource) : IDbConnectionFactory
{
    public async ValueTask<DbConnection> OpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        return await datasource.OpenConnectionAsync(cancellationToken);
    }
}
