using SharedKernel;

namespace Zylo.Domain.Friendships;

public sealed class Friendship : Entity, IAuditable
{
    private Friendship(Guid id, Guid userId, Guid friendId)
        : base(id)
    {
        Ensure.NotNullOrEmpty(userId, nameof(userId));
        Ensure.NotNullOrEmpty(friendId, nameof(friendId));

        UserId = userId;
        FriendId = friendId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Friendship"/> class.
    /// </summary>
    /// <remarks>
    /// Required by EF Core.
    /// </remarks>
    private Friendship()
    {
    }

    public Guid UserId { get; private set; }

    public Guid FriendId { get; private set; }
    
    public DateTime CreatedOnUtc { get; set; }

    public DateTime? ModifiedOnUtc { get; set; }

    public static Friendship Create(Guid userId, Guid friendId) =>
        new Friendship(Guid.NewGuid(), userId, friendId);
}
