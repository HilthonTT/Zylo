namespace Zylo.Contracts.FriendRequests;

public sealed record SentFriendRequestResponse(Guid Id, Guid FriendId, string FriendName, DateTime CreatedOnUtc);
