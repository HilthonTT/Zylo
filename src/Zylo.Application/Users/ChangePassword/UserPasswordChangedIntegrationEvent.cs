using Zylo.Application.Abstractions.Events;

namespace Zylo.Application.Users.ChangePassword;

public sealed record UserPasswordChangedIntegrationEvent(Guid Id) : IntegrationEvent(Id);
