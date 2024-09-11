using SharedKernel;

namespace Zylo.Domain.Notifications;

public sealed class Notification : Entity, IAuditable, ISoftDeletable
{
    internal Notification(Guid eventId, Guid userId, NotificationType notificationType, DateTime dateTimeUtc)
        : base(Guid.NewGuid())
    {
        Ensure.NotNullOrEmpty(eventId, nameof(eventId));
        Ensure.NotNullOrEmpty(userId, nameof(userId));
        Ensure.NotNull(dateTimeUtc, nameof(dateTimeUtc));

        EventId = eventId;
        UserId = userId;
        Type = notificationType;
        DateTimeUtc = dateTimeUtc;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Notification"/> class.
    /// </summary>
    /// <remarks>
    /// Required by EF Core.
    /// </remarks>
    private Notification()
    {
    }

    public Guid EventId { get; private set; }

    public Guid UserId { get; private set; }

    public NotificationType Type { get; private set; }

    public DateTime DateTimeUtc { get; private set; }

    public DateTime CreatedOnUtc { get; set; }

    public DateTime? ModifiedOnUtc { get; set; }

    public DateTime? DeletedOnUtc { get; set; }

    public bool IsDeleted { get; set; }
}
