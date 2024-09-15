using MediatR;

namespace Zylo.Application.Abstractions.Events;

public interface IIntegrationEventHandler<in TIntegrationEvent> : INotificationHandler<TIntegrationEvent>
    where TIntegrationEvent : IIntegrationEvent
{
}
