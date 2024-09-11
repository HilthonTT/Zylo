using SharedKernel;

namespace Zylo.Domain.Friendships.DomainEvents;

public sealed record FriendRequestRejectedDomainEvent(Guid FriendRequestId) : IDomainEvent;
