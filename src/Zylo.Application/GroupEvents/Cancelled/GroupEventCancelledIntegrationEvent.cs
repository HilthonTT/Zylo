using Zylo.Application.Abstractions.Events;

namespace Zylo.Application.GroupEvents.Cancelled;

public sealed record GroupEventCancelledIntegrationEvent(Guid Id) : IntegrationEvent(Id);
