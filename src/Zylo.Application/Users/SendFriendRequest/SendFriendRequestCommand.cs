using Zylo.Application.Abstractions.Messaging;

namespace Zylo.Application.Users.SendFriendRequest;

public sealed record SendFriendRequestCommand(Guid UserId, Guid FriendId) : ICommand;
