using Zylo.Application.Abstractions.Events;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Domain.Users.DomainEvents;

namespace Zylo.Application.Users.Verify;

internal class UserVerifiedEmailDomainEventHandler : IDomainEventHandler<UserEmailVerifiedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public UserVerifiedEmailDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public Task Handle(UserEmailVerifiedDomainEvent notification, CancellationToken cancellationToken)
    {
        return _eventBus.PublishAsync(new UserVerifiedEmailIntegrationEvent(notification.UserId), cancellationToken);
    }
}
