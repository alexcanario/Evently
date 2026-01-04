using Evently.Modules.Events.Application.Events.GetEvent;

namespace Evently.Modules.Events.Presentation.Events;

internal static class GetEvent
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events/{id:guid}", async (Guid id, ISender sender, IStringLocalizer<EventsResources> localizer) =>
        {
            EventResponse @event = await sender.Send(new GetEventQuery(id));

            return @event is null
                ? Results.NotFound(new { Message = localizer["EventNotFound"].Value })
                : Results.Ok(@event);
        }).WithTags(Tags.Events);
    }
}
