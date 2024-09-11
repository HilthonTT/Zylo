using SharedKernel;

namespace Zylo.Domain.Events;

public static class GroupEventErrors
{
    public static Error NotFound(Guid groupEventId) => Error.NotFound(
        "GroupEvent.NotFound",
        $"The group event with the Id = '{groupEventId}' was not found.");

    public static Error UserNotFound(Guid userId) => Error.NotFound(
        "GroupEvent.UserNotFound",
        $"The user with the Id = '{userId}' was not found.");

    public static Error FriendNotFound(Guid friendId) => Error.NotFound(
        "GroupEvent.FriendNotFound",
        $"The friend with the Id = '{friendId}' was not found.");

    public static readonly Error InvitationAlreadySent = Error.Conflict(
        "GroupEvent.InvitationAlreadySent",
        "The invitation for this event has already been sent to this user.");

    public static readonly Error NotFriends = Error.Problem(
        "GroupEvent.NotFriends",
        "The specified users are not friend.");

    public static readonly Error DateAndTimeIsInThePast = Error.Problem(
        "GroupEvent.InThePast",
        "The event date and time cannot be in the past.");
}
