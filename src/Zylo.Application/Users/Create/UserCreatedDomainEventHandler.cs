using Zylo.Application.Abstractions.Events;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Domain.Users.DomainEvents;

namespace Zylo.Application.Users.Create;

internal sealed class UserCreatedDomainEventHandler : IDomainEventHandler<UserCreatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public UserCreatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        return _eventBus.PublishAsync(new UserCreatedIntegrationEvent(notification.UserId), cancellationToken);
    }
}
