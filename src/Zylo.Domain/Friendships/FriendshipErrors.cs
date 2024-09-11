using SharedKernel;

namespace Zylo.Domain.Friendships;

public static class FriendshipErrors
{
    public static Error UserNotFound(Guid userId) => Error.NotFound(
        "Friendship.UserNotFound",
        $"The user with the Id = '{userId}' was not found.");

    public static Error FriendNotFound(Guid friendId) => Error.NotFound(
        "Friendship.FriendNotFound",
        $"The friend with the Id = '{friendId}' was not found.");

    public static readonly Error NotFriends = Error.Problem(
        "Friendship.NotFriends",
        "The specified users are not friend.");
}
