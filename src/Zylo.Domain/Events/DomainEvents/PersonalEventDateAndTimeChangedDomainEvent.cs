using SharedKernel;

namespace Zylo.Domain.Events.DomainEvents;

public sealed record PersonalEventDateAndTimeChangedDomainEvent(Guid PersonalEventId) : IDomainEvent;
