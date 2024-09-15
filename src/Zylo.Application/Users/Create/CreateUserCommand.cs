using Zylo.Application.Abstractions.Messaging;

namespace Zylo.Application.Users.Create;

public sealed record CreateUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : ICommand<Guid>;
