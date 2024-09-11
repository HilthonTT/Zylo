using SharedKernel;

namespace Zylo.Domain.Friendships.DomainEvents;

public sealed record FriendRequestSentDomainEvent(Guid FriendRequestId) : IDomainEvent;
