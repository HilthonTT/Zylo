namespace Zylo.Contracts.GroupEvents;

public sealed record DetailedGroupEventResponse(
    Guid Id,
    string Name,
    int CategoryId,
    string Category,
    string CreatedBy,
    int NumberOfAttendees,
    DateTime DateTimeUtc,
    DateTime CreatedOnUtc);
