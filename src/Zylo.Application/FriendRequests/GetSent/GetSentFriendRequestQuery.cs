using Zylo.Application.Abstractions.Messaging;
using Zylo.Contracts.FriendRequests;

namespace Zylo.Application.FriendRequests.GetSent;

public sealed record GetSentFriendRequestQuery(Guid UserId) : IQuery<List<SentFriendRequestResponse>>;
