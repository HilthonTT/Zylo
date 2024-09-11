using SharedKernel;

namespace Zylo.Domain.Friendships.DomainEvents;

public sealed record FriendshipRemovedDomainEvent(Guid FriendshipId) : IDomainEvent;
