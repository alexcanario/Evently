using Evently.Modules.Events.Domain.Events.Models;

namespace Evently.Modules.Events.Domain.Events.Abstractions;

public interface IEventRepository
{
    void Insert(Event @event);
    Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
