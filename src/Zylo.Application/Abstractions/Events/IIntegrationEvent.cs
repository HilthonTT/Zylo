using MediatR;

namespace Zylo.Application.Abstractions.Events;

public interface IIntegrationEvent : INotification
{
    Guid Id { get; init; }
}
