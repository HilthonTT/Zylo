using SharedKernel;
using Zylo.Domain.Users;

namespace Zylo.Domain.Friendships;

public sealed class FriendshipService
{
    private readonly IUserRepository _userRepository;
    private readonly IFriendshipRepository _friendshipRepository;

    public FriendshipService(
        IUserRepository userRepository,
        IFriendshipRepository friendshipRepository)
    {
        _userRepository = userRepository;
        _friendshipRepository = friendshipRepository;
    }

    public async Task CreateFriendshipAsync(FriendRequest friendRequest, CancellationToken cancellationToken = default)
    {
        if (friendRequest.Rejected)
        {
            throw new DomainException(FriendRequestErrors.AlreadyRejected);
        }

        User? user = await _userRepository.GetByIdAsync(friendRequest.UserId, cancellationToken);

        if (user is null)
        {
            throw new DomainException(FriendRequestErrors.UserNotFound(friendRequest.UserId));
        }

        User? friend = await _userRepository.GetByIdAsync(friendRequest.FriendId, cancellationToken);

        if (friend is null)
        {
            throw new DomainException(FriendRequestErrors.FriendNotFound(friendRequest.FriendId));
        }

        _friendshipRepository.Insert(Friendship.Create(user.Id, friend.Id));

        _friendshipRepository.Insert(Friendship.Create(friend.Id, user.Id));
    }

    public async Task<Result> RemoveFriendshipAsync(Guid userId, Guid friendId, CancellationToken cancellationToken = default)
    {
        User? user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            return Result.Failure(FriendRequestErrors.UserNotFound(userId));
        }

        User? friend = await _userRepository.GetByIdAsync(friendId);

        if (friend is null)
        {
            return Result.Failure(FriendRequestErrors.FriendNotFound(friendId));
        }

        if (!await _friendshipRepository.CheckIfFriendsAsync(user.Id, friend.Id, cancellationToken))
        {
            return Result.Failure(FriendshipErrors.NotFriends);
        }

        var userToFriendFriendship = Friendship.Create(user.Id, friend.Id);

        var friendToUserFriendship = Friendship.Create(friend.Id, user.Id);

        // This will add the appropriate domain event that will be published after saving the changes.
        user.RemoveFriendship(userToFriendFriendship);

        _friendshipRepository.Remove(userToFriendFriendship);

        _friendshipRepository.Remove(friendToUserFriendship);

        return Result.Success();
    }
}
