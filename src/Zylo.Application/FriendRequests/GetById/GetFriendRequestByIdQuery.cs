using Zylo.Application.Abstractions.Messaging;
using Zylo.Contracts.FriendRequests;

namespace Zylo.Application.FriendRequests.GetById;

public sealed record GetFriendRequestByIdQuery(Guid FriendRequestId) : IQuery<FriendRequestResponse>;
