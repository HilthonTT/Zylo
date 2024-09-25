using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Zylo.Application.Abstractions.Authentication;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Contracts.Common;
using Zylo.Contracts.Friendships;
using Zylo.Domain.Friendships;
using Zylo.Domain.Users;

namespace Zylo.Application.Friendships.GetByUserId;

internal sealed class GetFriendshipByUserIdQueryHandler 
    : IQueryHandler<GetFriendshipByUserIdQuery, PagedList<FriendshipResponse>>
{
    private readonly IUserContext _userContext;
    private readonly IDbContext _context;

    public GetFriendshipByUserIdQueryHandler(IUserContext userContext, IDbContext context)
    {
        _userContext = userContext;
        _context = context;
    }

    public async Task<Result<PagedList<FriendshipResponse>>> Handle(
        GetFriendshipByUserIdQuery request, 
        CancellationToken cancellationToken)
    {
        if (request.UserId != _userContext.UserId)
        {
            return Result.Failure<PagedList<FriendshipResponse>>(UserErrors.InvalidPermissions);
        }

        IQueryable<FriendshipResponse> query = _context.Friendships
           .AsNoTracking()
           .Where(friendship => friendship.UserId == request.UserId)
           .SelectMany(friendship => _context.Users
               .Where(user => user.Id == friendship.UserId)
               .SelectMany(user => _context.Users
                   .Where(friend => friend.Id == friendship.FriendId)
                   .Select(friend => new FriendshipResponse(
                       user.Id,
                       user.Email.Value,
                       user.FullName,
                       friend.Id,
                       friend.Email.Value,
                       friend.FullName,
                       friendship.CreatedOnUtc
                   ))));

        PagedList<FriendshipResponse> response = await PagedList<FriendshipResponse>.CreateAsync(
            query,
            request.Page,
            request.PageSize,
            cancellationToken);

        return response;
    }
}
