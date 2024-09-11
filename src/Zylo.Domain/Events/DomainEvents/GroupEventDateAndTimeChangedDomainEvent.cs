using SharedKernel;

namespace Zylo.Domain.Events.DomainEvents;

public sealed record GroupEventDateAndTimeChangedDomainEvent(
    Guid GroupEventId, 
    DateTime PreviousDateAndTime) : IDomainEvent;
