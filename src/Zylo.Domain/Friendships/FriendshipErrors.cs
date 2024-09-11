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

    public static readonly Error AlreadyFriends = Error.Conflict(
        "Friendship.AlreadyFriends",
        "The friendship request can not be sent because the users are already friends.");

    public static readonly Error PendingFriendshipRequest = Error.Conflict(
        "FriendshipRequest.PendingFriendshipRequest",
        "The friendship request can not be sent because there is a pending friendship request.");

    public static readonly Error AlreadyRejected = Error.Conflict(
        "FriendshipRequest.AlreadyRejected",
        "The friendship request has already been rejected.");
}
