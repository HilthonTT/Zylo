using Zylo.Application.Abstractions.Messaging;

namespace Zylo.Application.Users.Verify;

public sealed record VerifyUserCommand(int Code) : ICommand;
