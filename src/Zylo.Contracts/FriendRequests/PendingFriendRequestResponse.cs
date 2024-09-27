namespace Zylo.Contracts.FriendRequests;

public sealed record PendingFriendRequestResponse(Guid Id, Guid FriendId, string FriendName, DateTime CreatedOnUtc);
