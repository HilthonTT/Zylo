using Zylo.Application.Abstractions.Messaging;

namespace Zylo.Application.Users.ResendEmail;

public sealed record ResendUserEmailVerificationCommand(string Email) : ICommand;
