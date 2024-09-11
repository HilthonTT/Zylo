using SharedKernel;

namespace Zylo.Domain.Users.DomainEvents;

public sealed record UserNameChangedDomainEvent(Guid UserId) : IDomainEvent;
