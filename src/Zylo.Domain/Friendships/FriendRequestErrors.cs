using SharedKernel;

namespace Zylo.Domain.Friendships;

public static class FriendRequestErrors
{
    public static Error NotFound(Guid friendRequestId) => Error.NotFound(
        "FriendRequest.NotFound",
        $"The friendship request with the Id = '{friendRequestId}' was not found.");

    public static Error UserNotFound(Guid userId) => Error.NotFound(
        "FriendRequest.UserNotFound",
        $"The user with the Id = '{userId}' was not found.");

    public static Error FriendNotFound(Guid friendId) => Error.NotFound(
        "FriendRequest.FriendNotFound",
        $"The friend with the Id = '{friendId}' was not found.");

    public static readonly Error AlreadyAccepted = Error.Conflict(
        "FriendRequest.AlreadyAccepted",
        "The friendship request has already been accepted.");

    public static readonly Error AlreadyRejected = Error.Conflict(
        "FriendRequest.AlreadyRejected",
        "The friendship request has already been rejected.");

    public static readonly Error AlreadyFriends = Error.Conflict(
        "FriendRequest.AlreadyFriends",
        "The friendship request can not be sent because the users are already friends.");

    public static Error PendingFriendshipRequest = Error.Problem(
        "FriendRequest.PendingFriendshipRequest",
        "The friendship request can not be sent because there is a pending friendship request.");
}
