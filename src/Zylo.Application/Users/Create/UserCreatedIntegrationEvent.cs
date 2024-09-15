using Zylo.Application.Abstractions.Events;

namespace Zylo.Application.Users.Create;

public sealed record UserCreatedIntegrationEvent(Guid Id) : IntegrationEvent(Id);
