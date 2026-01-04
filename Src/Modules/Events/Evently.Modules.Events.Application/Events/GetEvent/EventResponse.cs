using Evently.Modules.Events.Domain.Events.Enums;

namespace Evently.Modules.Events.Application.Events.GetEvent;

public sealed record EventResponse(Guid Id, string Title, string Description, string Location, DateTime StartsAtUtc, DateTime? EndsAtUtc, EventStatus Status);
