using Zylo.Domain.Events;
using Zylo.Domain.Friendships;
using Zylo.Domain.Users;

namespace Zylo.Domain.Invitations;

public interface IInvitationRepository
{
    Task<Invitation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> HasAlreadySentAsync(GroupEvent groupEvent, User user, CancellationToken cancellationToken = default);

    void Insert(Invitation invitation);

    Task RemovePendingInvitationsForFriendshipAsync(
        Friendship friendship, 
        DateTime utcNow, 
        CancellationToken cancellationToken = default);

    Task RemoveInvitationsForGroupEventAsync(
        Guid groupEventId, 
        DateTime utcNow,
        CancellationToken cancellationToken = default);
}
