using Zylo.Application.Abstractions.Messaging;

namespace Zylo.Application.Users.ChangePassword;

public sealed record ChangePasswordCommand(Guid UserId, string CurrentPassword, string NewPassword) : ICommand;
