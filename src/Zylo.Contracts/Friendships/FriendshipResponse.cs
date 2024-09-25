namespace Zylo.Contracts.Friendships;

public sealed record FriendshipResponse(
    Guid UserId, 
    string UserEmail, 
    string UserName, 
    Guid FriendId, 
    string FriendEmail, 
    string FriendName, 
    DateTime CreatedOnUtc);
