using Zylo.Application.Abstractions.Messaging;
using Zylo.Contracts.FriendRequests;

namespace Zylo.Application.FriendRequests.GetPending;

public sealed record GetPendingFriendRequestsQuery(Guid UserId) : IQuery<List<PendingFriendRequestResponse>>;
