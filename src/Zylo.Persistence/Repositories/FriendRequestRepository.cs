using Microsoft.EntityFrameworkCore;
using Zylo.Domain.Friendships;

namespace Zylo.Persistence.Repositories;

internal sealed class FriendRequestRepository : IFriendRequestRepository
{
    private readonly ZyloDbContext _context;

    public FriendRequestRepository(ZyloDbContext context)
    {
        _context = context;
    }

    public Task<FriendRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.FriendRequests.FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    public Task<bool> HasPendingFriendRequestAsync(Guid userId, Guid friendId, CancellationToken cancellationToken = default)
    {
        return _context.FriendRequests
            .AnyAsync(f => 
                (f.UserId == userId || f.FriendId == friendId) && 
                (f.FriendId == userId || f.UserId == friendId) && 
                f.CompletedOnUtc == null,
                cancellationToken);
    }

    public void Insert(FriendRequest friendRequest)
    {
        _context.FriendRequests.Add(friendRequest);
    }
}
