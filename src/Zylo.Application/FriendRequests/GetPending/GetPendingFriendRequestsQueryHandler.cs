using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Zylo.Application.Abstractions.Authentication;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Contracts.FriendRequests;
using Zylo.Domain.Users;

namespace Zylo.Application.FriendRequests.GetPending;

internal sealed class GetPendingFriendRequestsQueryHandler
    : IQueryHandler<GetPendingFriendRequestsQuery, List<PendingFriendRequestResponse>>
{
    private readonly IUserContext _userContext;
    private readonly IDbContext _context;

    public GetPendingFriendRequestsQueryHandler(
        IUserContext userContext, 
        IDbContext context)
    {
        _userContext = userContext;
        _context = context;
    }

    public async Task<Result<List<PendingFriendRequestResponse>>> Handle(
        GetPendingFriendRequestsQuery request, 
        CancellationToken cancellationToken)
    {
        if (request.UserId != _userContext.UserId)
        {
            return Result.Failure<List<PendingFriendRequestResponse>>(UserErrors.InvalidPermissions);
        }

        List<PendingFriendRequestResponse> friendshipRequests = await _context.FriendRequests
            .AsNoTracking()
            .Join(
                _context.Users.AsNoTracking(),
                friendshipRequest => friendshipRequest.UserId,
                user => user.Id,
                (friendshipRequest, user) => new { friendshipRequest, user }
            )
            .Where(result => result.friendshipRequest.FriendId == request.UserId && result.friendshipRequest.CompletedOnUtc == null)
            .Select(result => new PendingFriendRequestResponse
            (
                result.friendshipRequest.Id,
                result.user.Id,
                result.user.FullName,
                result.friendshipRequest.CreatedOnUtc
            ))
            .ToListAsync(cancellationToken);

        return friendshipRequests;
    }
}
