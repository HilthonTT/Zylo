using Zylo.Application.Abstractions.Messaging;
using Zylo.Contracts.Friendships;

namespace Zylo.Application.Friendships.Get;

public sealed record GetFriendshipQuery(Guid UserId, Guid FriendId) : IQuery<FriendshipResponse>;
