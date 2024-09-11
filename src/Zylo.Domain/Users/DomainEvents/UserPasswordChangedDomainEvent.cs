using SharedKernel;

namespace Zylo.Domain.Users.DomainEvents;

public sealed record UserPasswordChangedDomainEvent(Guid UserId) : IDomainEvent;
