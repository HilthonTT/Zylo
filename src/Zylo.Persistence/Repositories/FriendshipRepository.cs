using Microsoft.EntityFrameworkCore;
using Zylo.Domain.Friendships;

namespace Zylo.Persistence.Repositories;

internal sealed class FriendshipRepository : IFriendshipRepository
{
    private readonly ZyloDbContext _context;

    public FriendshipRepository(ZyloDbContext context)
    {
        _context = context;
    }

    public Task<Friendship?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Friendships.FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    public Task<bool> CheckIfFriendsAsync(Guid userId, Guid friendId, CancellationToken cancellationToken = default)
    {
        return _context.Friendships.AnyAsync(f => f.UserId == userId && f.FriendId == friendId, cancellationToken);
    }

    public void Insert(Friendship friendship)
    {
        _context.Friendships.Add(friendship);
    }

    public void Remove(Friendship friendship)
    {
        _context.Friendships.Remove(friendship);
    }
}
