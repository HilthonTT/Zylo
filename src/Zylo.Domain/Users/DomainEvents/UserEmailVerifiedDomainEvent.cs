using SharedKernel;

namespace Zylo.Domain.Users.DomainEvents;

public sealed record UserEmailVerifiedDomainEvent(Guid UserId) : IDomainEvent;
