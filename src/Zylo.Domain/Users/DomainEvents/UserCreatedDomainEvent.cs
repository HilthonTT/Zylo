using SharedKernel;

namespace Zylo.Domain.Users.DomainEvents;

public sealed record UserCreatedDomainEvent(Guid UserId) : IDomainEvent;
