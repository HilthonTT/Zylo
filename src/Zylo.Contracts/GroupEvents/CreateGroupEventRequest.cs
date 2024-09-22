namespace Zylo.Contracts.GroupEvents;

public sealed record CreateGroupEventRequest(Guid UserId, string Name, int CategoryId, DateTime DateTime);
