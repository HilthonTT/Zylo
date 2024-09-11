using SharedKernel;

namespace Zylo.Domain.Invitations;

public static class InvitationErrors
{
    public static Error NotFound(Guid invitationId) => Error.NotFound(
        "Invitation.NotFound",
        $"The invitation with the Id = '{invitationId}' was not found.");

    public static Error EventNotFound(Guid eventId) => Error.NotFound(
        "Invitation.EventNotFound",
        $"The event with the Id = '{eventId}' was not found.");

    public static Error UserNotFound(Guid userId) => Error.NotFound(
        "Invitation.UserNotFound",
        $"The user with the Id = '{userId}' was not found.");

    public static Error FriendNotFound(Guid friendId) => Error.NotFound(
        "Invitation.FriendNotFound",
        $"The friend with the Id = '{friendId}' was not found.");

    public static readonly Error AlreadyAccepted = Error.Conflict(
        "Invitation.AlreadyAccepted", 
        "The invitation has already been accepted.");

    public static readonly Error AlreadyRejected = Error.Conflict(
        "Invitation.AlreadyRejected", 
        "The invitation has already been rejected.");
}
