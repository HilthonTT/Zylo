using SharedKernel;

namespace Zylo.Domain.Events.DomainEvents;

public sealed record PersonalEventCreatedDomainEvent(Guid PersonalEventId) : IDomainEvent;
