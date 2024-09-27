using Zylo.Application.Abstractions.Messaging;

namespace Zylo.Application.FriendRequests.Reject;

public sealed record RejectFriendRequestCommand(Guid FriendRequestId) : ICommand;
