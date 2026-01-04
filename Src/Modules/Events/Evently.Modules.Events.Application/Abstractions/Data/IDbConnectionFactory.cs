using System.Data.Common;

namespace Evently.Modules.Events.Application.Abstraction.Data;

public interface IDbConnectionFactory
{
    ValueTask<DbConnection> OpenConnectionAsync(CancellationToken cancellationToken = default);
}
