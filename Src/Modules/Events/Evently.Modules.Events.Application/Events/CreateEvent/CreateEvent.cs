using Evently.Modules.Events.Application.Abstraction.Data;
using Evently.Modules.Events.Domain.Events.Abstractions;
using Evently.Modules.Events.Domain.Events.Enums;
using Evently.Modules.Events.Domain.Events.Models;

using MediatR;

namespace Evently.Modules.Events.Application.Events.CreateEvent;

public sealed class CreateEventCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateEventCommand, Guid>
{
    public async Task<Guid> Handle(CreateEventCommand command, CancellationToken cancellationToken)
    {
        var @event = new Event
        {
            Id = Guid.NewGuid(),
            Title = command.Title,
            Description = command.Description,
            Location = command.Location,
            StartsAtUtc = DateTime.SpecifyKind(command.StartsAtUtc, DateTimeKind.Utc),
            EndsAtUtc = command.EndsAtUtc is null ? null : DateTime.SpecifyKind(command.EndsAtUtc!.Value, DateTimeKind.Utc),
            Status = EventStatus.Draft,
        };

        eventRepository.Insert(@event);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return @event.Id;
    }
}
