namespace Zylo.Domain.Friendships;

public interface IFriendRequestRepository
{
    Task<FriendRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> HasPendingFriendRequestAsync(Guid userId, Guid friendId, CancellationToken cancellationToken = default);

    void Insert(FriendRequest friendRequest);
}
