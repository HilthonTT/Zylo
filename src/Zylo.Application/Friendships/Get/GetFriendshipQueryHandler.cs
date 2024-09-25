using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Zylo.Application.Abstractions.Authentication;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Contracts.Friendships;
using Zylo.Domain.Friendships;
using Zylo.Domain.Users;

namespace Zylo.Application.Friendships.Get;

internal sealed class GetFriendshipQueryHandler : IQueryHandler<GetFriendshipQuery, FriendshipResponse>
{
    private readonly IUserContext _userContext;
    private readonly IDbContext _context;

    public GetFriendshipQueryHandler(IUserContext userContext, IDbContext context)
    {
        _userContext = userContext;
        _context = context;
    }

    public async Task<Result<FriendshipResponse>> Handle(GetFriendshipQuery request, CancellationToken cancellationToken)
    {
        if (request.UserId != _userContext.UserId)
        {
            return Result.Failure<FriendshipResponse>(UserErrors.InvalidPermissions);
        }

        FriendshipResponse? response = await _context.Friendships
            .AsNoTracking()
            .Join(
                _context.Users.AsNoTracking(),
                friendship => friendship.UserId,
                user => user.Id,
                (friendship, user) => new { friendship, user }
            )
            .Join(
                _context.Users.AsNoTracking(),
                combined => combined.friendship.FriendId,
                friend => friend.Id,
                (combined, friend) => new { combined.friendship, combined.user, friend }
            )
            .Where(result => result.friendship.UserId == request.UserId && result.friendship.FriendId == request.FriendId)
            .Select(result => new FriendshipResponse
            (
                result.user.Id,
                result.user.Email.Value,
                result.user.FullName,
                result.friend.Id,
                result.friend.Email.Value,
                result.friend.FullName,
                result.friendship.CreatedOnUtc
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (response is null)
        {
            return Result.Failure<FriendshipResponse>(FriendshipErrors.NotFriends);
        }

        if (response.UserId != _userContext.UserId || response.FriendId != _userContext.UserId)
        {
            return Result.Failure<FriendshipResponse>(FriendshipErrors.NotFriends);
        }

        return response;
    }
}
