using SharedKernel;

namespace Zylo.Domain.Events.DomainEvents;

public sealed record GroupEventCreatedDomainEvent(Guid GroupEventId) : IDomainEvent;
