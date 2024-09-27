namespace Zylo.Contracts.FriendRequests;

public sealed record FriendRequestResponse(
    Guid UserId, 
    string UserEmail,
    string UserName,
    Guid FriendId,
    string FriendEmail,
    string FriendName,
    DateTime CreatedOnUtc);
