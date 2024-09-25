using Zylo.Application.Abstractions.Messaging;

namespace Zylo.Application.FriendRequests.Accept;

public sealed record AcceptFriendRequestCommand(Guid FriendRequestId) : ICommand;
