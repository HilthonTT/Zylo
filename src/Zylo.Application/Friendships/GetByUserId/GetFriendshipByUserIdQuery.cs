using Zylo.Application.Abstractions.Messaging;
using Zylo.Contracts.Common;
using Zylo.Contracts.Friendships;

namespace Zylo.Application.Friendships.GetByUserId;

public sealed record GetFriendshipByUserIdQuery(Guid UserId, int Page, int PageSize) : IQuery<PagedList<FriendshipResponse>>;
