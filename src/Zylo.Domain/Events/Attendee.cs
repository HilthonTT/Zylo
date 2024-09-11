using SharedKernel;
using Zylo.Domain.Invitations;

namespace Zylo.Domain.Events;

public sealed class Attendee : Entity, IAuditable, ISoftDeletable
{
    public Attendee(Invitation invitation)
        : base(Guid.NewGuid())
    {
        Ensure.NotNull(invitation, nameof(invitation));
        Ensure.NotNullOrEmpty(invitation.EventId, nameof(invitation.EventId));
        Ensure.NotNullOrEmpty(invitation.UserId, nameof(invitation.UserId));

        EventId = invitation.EventId;
        UserId = invitation.UserId;
    }

    internal Attendee(GroupEvent groupEvent)
    {
        Ensure.NotNull(groupEvent, nameof(groupEvent));
        Ensure.NotNullOrEmpty(groupEvent.Id, nameof(groupEvent.Id));
        Ensure.NotNullOrEmpty(groupEvent.UserId, nameof(groupEvent.UserId));

        EventId = groupEvent.Id;
        UserId = groupEvent.UserId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Attendee"/> class.
    /// </summary>
    /// <remarks>
    /// Required by EF Core.
    /// </remarks>
    private Attendee()
    {
    }

    public Guid EventId { get; private set; }

    public Guid UserId { get; private set; }

    public bool Processed { get; private set; }

    public DateTime CreatedOnUtc { get; set; }

    public DateTime? ModifiedOnUtc { get; set; }

    public DateTime? DeletedOnUtc { get; set; }

    public bool IsDeleted { get; set; }

    public Result MarkAsProcessed()
    {
        if (Processed)
        {
            return Result.Failure(AttendeeErrors.AlreadyProcessed);
        }

        Processed = true;

        return Result.Success();
    }
}
