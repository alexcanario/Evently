using Evently.Modules.Events.Application.Events.GetEvent;
using Evently.Modules.Events.Domain.Events.Models;

namespace Evently.Modules.Events.Application.Mappers;

public static class EventMapper
{
    public static EventResponse MapToEventResponse(this Event @event)
    {
        return new EventResponse(
            @event.Id,
            @event.Title,
            @event.Description,
            @event.Location,
            @event.StartsAtUtc,
            @event.EndsAtUtc,
            @event.Status);
    }
}
