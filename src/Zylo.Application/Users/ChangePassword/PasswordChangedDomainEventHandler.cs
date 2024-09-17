using Zylo.Application.Abstractions.Events;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Domain.Users.DomainEvents;

namespace Zylo.Application.Users.ChangePassword;

internal sealed class PasswordChangedDomainEventHandler : IDomainEventHandler<UserPasswordChangedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public PasswordChangedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public Task Handle(UserPasswordChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        return _eventBus.PublishAsync(new UserPasswordChangedIntegrationEvent(notification.UserId), cancellationToken);
    }
}
