using Evently.Modules.Events.Api.Database;
using Evently.Modules.Events.Api.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;

namespace Evently.Modules.Events.Api.Events;

internal sealed record CreateEventRequest(string Title, string Description, string Location, DateTime StartsAtUtc, DateTime EndsAtUtc);

public static class CreateEvent
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("events", async (
            CreateEventRequest request, 
            EventsDbContext context,
            IStringLocalizer<EventsResources> localizer,
            HttpContext httpContext) =>
        {
            var @event = new Event
            {
                Title = request.Title,
                Description = request.Description,
                Location = request.Location,
                StartsAtUtc = request.StartsAtUtc,
                EndsAtUtc = request.EndsAtUtc,
                Status = EventStatus.Draft,
            };

            context.Events.Add(@event);
            
            await context.SaveChangesAsync();

            return Results.Created($"/events/{@event.Id}", new 
            { 
                @event.Id,
                Message = localizer["EventCreatedWithTitleAt", @event.Id, @event.Title, @event.StartsAtUtc.ToLocalTime()].Value
            });
        })
        .WithTags(Tags.Events)
        .WithName("CreateEvent");
    }
}
