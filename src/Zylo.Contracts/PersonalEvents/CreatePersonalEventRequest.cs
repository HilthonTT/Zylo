namespace Zylo.Contracts.PersonalEvents;

public sealed record CreatePersonalEventRequest(Guid UserId, string Name, int CategoryId, DateTime DateTimeUtc);
