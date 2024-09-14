namespace Zylo.Persistence.Outbox;

public sealed record OutboxMessage(
    Guid Id,
    string Name, 
    string Content, 
    DateTime CreatedOnUtc, 
    DateTime? Processed = null, 
    string? Error = null);
