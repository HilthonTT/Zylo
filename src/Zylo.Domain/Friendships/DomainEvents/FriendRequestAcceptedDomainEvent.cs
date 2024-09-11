using SharedKernel;

namespace Zylo.Domain.Friendships.DomainEvents;

public sealed record FriendRequestAcceptedDomainEvent(Guid FriendRequestId) : IDomainEvent;
