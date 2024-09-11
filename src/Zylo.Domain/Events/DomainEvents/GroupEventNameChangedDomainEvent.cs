﻿using SharedKernel;

namespace Zylo.Domain.Events.DomainEvents;

public sealed record GroupEventNameChangedDomainEvent(
    Guid GroupEventId,
    string PreviousName) : IDomainEvent;
