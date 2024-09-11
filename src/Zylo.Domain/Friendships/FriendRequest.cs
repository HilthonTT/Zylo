using SharedKernel;
using Zylo.Domain.Friendships.DomainEvents;

namespace Zylo.Domain.Friendships;

public sealed class FriendRequest : Entity, IAuditable, ISoftDeletable
{
    public FriendRequest(Guid userId, Guid friendId)
        : base(Guid.NewGuid())
    {
        Ensure.NotNullOrEmpty(userId, nameof(userId));
        Ensure.NotNullOrEmpty(friendId, nameof(friendId));

        UserId = userId;
        FriendId = friendId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FriendRequest"/> class.
    /// </summary>
    /// <remarks>
    /// Required by EF Core.
    /// </remarks>
    private FriendRequest()
    {
    }

    public Guid UserId { get; private set; }

    public Guid FriendId { get; private set; }

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
            return Result.Failure(FriendRequestErrors.AlreadyAccepted);
        }

        if (Rejected)
        {
            return Result.Failure(FriendRequestErrors.AlreadyRejected);
        }

        Accepted = true;

        CompletedOnUtc = utcNow;

        Raise(new FriendRequestAcceptedDomainEvent(Id));

        return Result.Success();
    }

    public Result Reject(DateTime utcNow)
    {
        if (Accepted)
        {
            return Result.Failure(FriendRequestErrors.AlreadyAccepted);
        }

        if (Rejected)
        {
            return Result.Failure(FriendRequestErrors.AlreadyRejected);
        }

        Rejected = true;

        CompletedOnUtc = utcNow;

        Raise(new FriendRequestRejectedDomainEvent(Id));

        return Result.Success();
    }
}
