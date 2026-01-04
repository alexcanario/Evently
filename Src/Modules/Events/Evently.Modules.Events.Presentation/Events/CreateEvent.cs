using Evently.Modules.Events.Application.Events.CreateEvent;

namespace Evently.Modules.Events.Presentation.Events;

internal static class CreateEvent
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("events", async (CreateEventRequest request, ISender sender, IStringLocalizer<EventsResources> localizer) =>
        {
            var command = new CreateEventCommand(
                request.Title,
                request.Description,
                request.Location,
                DateTime.SpecifyKind(request.StartsAtUtc, DateTimeKind.Utc),
                DateTime.SpecifyKind(request.EndsAtUtc, DateTimeKind.Utc));

            Guid eventId = await sender.Send(command);

            return Results.Created($"/events/{eventId}", new 
            { 
                eventId,
                Message = localizer["EventCreatedWithTitleAt", eventId, command.Title, command.StartsAtUtc.ToLocalTime()].Value
            });
        })
        .WithTags(Tags.Events)
        .WithName("CreateEvent");
    }

    private sealed record CreateEventRequest(string Title, string Description, string Location, DateTime StartsAtUtc, DateTime EndsAtUtc);
}
