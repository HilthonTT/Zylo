using Zylo.Application.Abstractions.Events;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Domain.Friendships.DomainEvents;

namespace Zylo.Application.FriendRequests.Sent;

internal sealed class FriendRequestSentDomainEventHandler : IDomainEventHandler<FriendRequestSentDomainEvent>
{
    private readonly IEventBus _eventBus;

    public FriendRequestSentDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public Task Handle(FriendRequestSentDomainEvent notification, CancellationToken cancellationToken)
    {
        return _eventBus.PublishAsync(new FriendRequestSentIntegrationEvent(notification.FriendRequestId), cancellationToken);
    }
}
