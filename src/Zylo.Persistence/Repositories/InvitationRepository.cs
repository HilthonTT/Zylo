using Microsoft.EntityFrameworkCore;
using Zylo.Domain.Events;
using Zylo.Domain.Friendships;
using Zylo.Domain.Invitations;
using Zylo.Domain.Users;

namespace Zylo.Persistence.Repositories;

internal sealed class InvitationRepository : IInvitationRepository
{
    private readonly ZyloDbContext _context;

    public InvitationRepository(ZyloDbContext context)
    {
        _context = context;
    }

    public Task<Invitation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Invitations.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public Task<bool> HasAlreadySentAsync(GroupEvent groupEvent, User user, CancellationToken cancellationToken = default)
    {
        return _context.Invitations.AnyAsync(i => 
            i.CompletedOnUtc == null && 
            i.EventId == groupEvent.Id && 
            i.UserId == user.Id, cancellationToken);
    }

    public void Insert(Invitation invitation)
    {
        _context.Invitations.Add(invitation);
    }

    public Task RemoveInvitationsForGroupEventAsync(
        GroupEvent groupEvent, 
        DateTime utcNow, 
        CancellationToken cancellationToken = default)
    {
        return _context.Invitations
            .Where(i => i.EventId == groupEvent.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(i => i.DeletedOnUtc, utcNow)
                .SetProperty(i => i.IsDeleted, true),
                cancellationToken);
    }

    public Task RemovePendingInvitationsForFriendshipAsync(
        Friendship friendship, 
        DateTime utcNow, 
        CancellationToken cancellationToken = default)
    {
        return _context.Invitations
            .Where(i => i.UserId == friendship.UserId || i.UserId == friendship.FriendId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(i => i.DeletedOnUtc, utcNow)
                .SetProperty(i => i.IsDeleted, true),
                cancellationToken);
    }
}
