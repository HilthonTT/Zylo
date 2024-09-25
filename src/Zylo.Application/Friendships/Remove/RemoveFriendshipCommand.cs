using Zylo.Application.Abstractions.Messaging;

namespace Zylo.Application.Friendships.Remove;

public sealed record RemoveFriendshipCommand(Guid UserId, Guid FriendId) : ICommand;
