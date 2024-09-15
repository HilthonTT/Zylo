using Zylo.Application.Abstractions.Events;

namespace Zylo.Application.Users.Verify;

public sealed record UserVerifiedEmailIntegrationEvent(Guid Id) : IntegrationEvent(Id);
