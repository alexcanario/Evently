namespace Evently.Modules.Events.Application.Abstraction.Data;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
