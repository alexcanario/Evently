using Evently.Modules.Events.Application.Mappers;
using Evently.Modules.Events.Domain.Events.Abstractions;
using Evently.Modules.Events.Domain.Events.Models;
using MediatR;

namespace Evently.Modules.Events.Application.Events.GetEvent;

public sealed class GetEventHandler(IEventRepository eventRepository) : IRequestHandler<GetEventQuery, EventResponse?>
{
    public async Task<EventResponse?> Handle(GetEventQuery query, CancellationToken cancellationToken)
    {
        Event? @event = await eventRepository.GetByIdAsync(query.Id, cancellationToken);
        return @event?.MapToEventResponse();
    }
}
