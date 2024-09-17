namespace Zylo.Contracts.Users;

public sealed record SendFriendRequestRequest(Guid UserId, Guid FriendId);
