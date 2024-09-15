using Zylo.Application.Abstractions.Messaging;
using Zylo.Contracts.Authentication;

namespace Zylo.Application.Users.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<TokenResponse>;
