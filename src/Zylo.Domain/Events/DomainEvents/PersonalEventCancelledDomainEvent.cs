using SharedKernel;

namespace Zylo.Domain.Events.DomainEvents;

public sealed record PersonalEventCancelledDomainEvent(Guid PersonalEventId) : IDomainEvent;
