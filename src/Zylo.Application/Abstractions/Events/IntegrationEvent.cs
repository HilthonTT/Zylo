namespace Zylo.Application.Abstractions.Events;

public abstract record IntegrationEvent(Guid Id) : IIntegrationEvent;
