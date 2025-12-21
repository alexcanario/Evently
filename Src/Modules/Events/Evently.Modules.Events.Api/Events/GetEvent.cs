using Evently.Modules.Events.Api.Database;
using Evently.Modules.Events.Api.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;

namespace Evently.Modules.Events.Api.Events;

public static class GetEvent
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events/{id}", (
            Guid id, 
            EventsDbContext context,
            IStringLocalizer<EventsResources> localizer) =>
        {
            EventResponse? @event = context.Events
                .Where(e => e.Id == id)
                .Select(e => new EventResponse(
                    e.Id,
                    e.Title,
                    e.Description,
                    e.Location,
                    e.StartsAtUtc,
                    e.EndsAtUtc,
                    e.Status))
                .FirstOrDefault();

            return @event is null 
                ? Results.NotFound(new { Message = localizer["EventNotFound"].Value })
                : Results.Ok(@event);
        }).WithTags(Tags.Events);
    }
}
