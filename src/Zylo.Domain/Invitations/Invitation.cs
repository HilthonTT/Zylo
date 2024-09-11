using SharedKernel;
using Zylo.Domain.Invitations.DomainEvents;

namespace Zylo.Domain.Invitations;

public sealed class Invitation : Entity, IAuditable, ISoftDeletable
{
    public Invitation(Guid eventId, Guid userId)
            : base(Guid.NewGuid())
    {
        Ensure.NotNullOrEmpty(eventId, nameof(eventId));
        Ensure.NotNullOrEmpty(userId, nameof(userId));

        EventId = eventId;
        UserId = userId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Invitation"/> class.
    /// </summary>
    /// <remarks>
    /// Required by EF Core.
    /// </remarks>
    private Invitation()
    {
    }

    public Guid EventId { get; private set; }

    public Guid UserId { get; private set; }

    public bool Accepted { get; private set; }

    public bool Rejected { get; private set; }

    public DateTime? CompletedOnUtc { get; private set; }

    public DateTime CreatedOnUtc { get; set; }

    public DateTime? ModifiedOnUtc { get; set; }

    public DateTime? DeletedOnUtc { get; set; }

    public bool IsDeleted { get; set; }

    public Result Accept(DateTime utcNow)
    {
        if (Accepted)
        {
            return Result.Failure(InvitationErrors.AlreadyAccepted);
        }

        if (Rejected)
        {
            return Result.Failure(InvitationErrors.AlreadyRejected);
        }

        Accepted = true;

        CompletedOnUtc = utcNow;

        Raise(new InvitationAcceptedDomainEvent(Id));

        return Result.Success();
    }

    public Result Reject(DateTime utcNow)
    {
        if (Accepted)
        {
            return Result.Failure(InvitationErrors.AlreadyAccepted);
        }

        if (Rejected)
        {
            return Result.Failure(InvitationErrors.AlreadyRejected);
        }

        Rejected = true;

        CompletedOnUtc = utcNow;

        Raise(new InvitationRejectedDomainEvent(Id));

        return Result.Success();
    }
}
