using Zylo.Application.Abstractions.Messaging;

namespace Zylo.Application.Users.Update;

public sealed record UpdateUserCommand(Guid UserId, string FirstName, string LastName) : ICommand;
