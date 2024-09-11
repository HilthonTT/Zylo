namespace Zylo.Domain.Friendships;

public interface IFriendshipRepository
{
    Task<bool> CheckIfFriendsAsync(Guid userId, Guid friendId, CancellationToken cancellationToken = default);

    void Insert(Friendship friendship);

    void Remove(Friendship friendship);
}
