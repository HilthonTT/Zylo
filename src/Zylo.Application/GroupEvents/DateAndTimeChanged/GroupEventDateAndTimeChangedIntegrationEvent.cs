using Zylo.Application.Abstractions.Events;

namespace Zylo.Application.GroupEvents.DateAndTimeChanged;

public sealed record GroupEventDateAndTimeChangedIntegrationEvent(Guid Id) : IntegrationEvent(Id);
