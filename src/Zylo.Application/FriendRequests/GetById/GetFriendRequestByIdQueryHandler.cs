using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Zylo.Application.Abstractions.Authentication;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Contracts.FriendRequests;
using Zylo.Domain.Friendships;
using Zylo.Domain.Users;

namespace Zylo.Application.FriendRequests.GetById;

internal sealed class GetFriendRequestByIdQueryHandler : IQueryHandler<GetFriendRequestByIdQuery, FriendRequestResponse>
{
    private readonly IUserContext _userContext;
    private readonly IDbContext _context;

    public GetFriendRequestByIdQueryHandler(IUserContext userContext, IDbContext context)
    {
        _userContext = userContext;
        _context = context;
    }

    public async Task<Result<FriendRequestResponse>> Handle(
        GetFriendRequestByIdQuery request, 
        CancellationToken cancellationToken)
    {
        FriendRequestResponse? response = await _context.FriendRequests
            .AsNoTracking()
            .Join(
                _context.Users.AsNoTracking(),
                friendshipRequest => friendshipRequest.UserId,
                user => user.Id,
                (friendshipRequest, user) => new { friendshipRequest, user }
            )
            .Join(
                _context.Users.AsNoTracking(),
                result => result.friendshipRequest.FriendId,
                friend => friend.Id,
                (result, friend) => new { result.friendshipRequest, result.user, friend }
            )
            .Where(x => x.friendshipRequest.Id == request.FriendRequestId && x.friendshipRequest.CompletedOnUtc == null)
            .Select(x => new FriendRequestResponse
            (
                x.user.Id,
                x.user.Email.Value,
                x.user.FullName,
                x.friend.Id,
                x.friend.Email.Value,
                x.friend.FullName,
                x.friendshipRequest.CreatedOnUtc
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (response is null)
        {
            return Result.Failure<FriendRequestResponse>(FriendRequestErrors.NotFound(request.FriendRequestId));
        }

        if (response.UserId != _userContext.UserId || response.FriendId != _userContext.UserId)
        {
            return Result.Failure<FriendRequestResponse>(UserErrors.InvalidPermissions);
        }

        return response;
    }
}
