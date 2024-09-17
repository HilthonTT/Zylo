using SharedKernel;
using Zylo.Domain.Friendships;
using Zylo.Domain.Friendships.DomainEvents;
using Zylo.Domain.Users.DomainEvents;
using Zylo.Domain.Users.ValueObjects;

namespace Zylo.Domain.Users;

public sealed class User : Entity, IAuditable, ISoftDeletable
{
    private User(Guid id, Email email, FirstName firstName, LastName lastName, string passwordHash)
        : base(id)
    {
        Ensure.NotNullOrEmpty(id, nameof(id));
        Ensure.NotNullOrEmpty(email, nameof(email));
        Ensure.NotNullOrEmpty(firstName, nameof(firstName));
        Ensure.NotNullOrEmpty(lastName, nameof(lastName));
        Ensure.NotNullOrEmpty(passwordHash, nameof(passwordHash));

        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PasswordHash = passwordHash;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <remarks>
    /// Required by EF Core.
    /// </remarks>
    private User()
    {
    }

    public Email Email { get; private set; }

    public FirstName FirstName { get; private set; }

    public LastName LastName { get; private set; }

    public string FullName => $"{FirstName.Value} {LastName.Value}";

    public string PasswordHash { get; private set; }

    public bool EmailVerified { get; private set; }

    public DateTime CreatedOnUtc { get; set; }

    public DateTime? ModifiedOnUtc { get; set; }

    public DateTime? DeletedOnUtc { get; set; }

    public bool IsDeleted { get; set; }

    public static User Create(Email email, FirstName firstName, LastName lastName, string passwordHash)
    {
        var user = new User(Guid.NewGuid(), email, firstName, lastName, passwordHash);

        user.Raise(new UserCreatedDomainEvent(user.Id));

        return user;
    }

    public Result ChangePassword(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            return Result.Failure(PasswordErrors.Empty);
        }

        if (passwordHash == PasswordHash)
        {
            return Result.Failure(UserErrors.CannotChangePassword);
        }

        PasswordHash = passwordHash;

        Raise(new UserPasswordChangedDomainEvent(Id));

        return Result.Success();
    }

    public Result ChangeName(FirstName firstName, LastName lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return Result.Failure(FirstNameErrors.Empty);
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            return Result.Failure(LastNameErrors.Empty);
        }

        FirstName = firstName;

        LastName = lastName;

        Raise(new UserNameChangedDomainEvent(Id));

        return Result.Success();
    }

    public void VerifyEmail()
    {
        EmailVerified = true;

        Raise(new UserEmailVerifiedDomainEvent(Id));
    }

    internal void RemoveFriendship(Friendship friendship)
    {
        Raise(new FriendshipRemovedDomainEvent(friendship.Id));
    }

    public async Task<Result<FriendRequest>> SendFriendshipRequestAsync(
        Guid friendId,
        IFriendshipRepository friendshipRepository,
        IFriendRequestRepository friendRequestRepository,
        CancellationToken cancellationToken = default)
    {
        if (await friendshipRepository.CheckIfFriendsAsync(Id, friendId, cancellationToken))
        {
            return Result.Failure<FriendRequest>(FriendshipErrors.AlreadyFriends);
        }

        if (await friendRequestRepository.HasPendingFriendRequestAsync(Id, friendId, cancellationToken))
        {
            return Result.Failure<FriendRequest>(FriendshipErrors.PendingFriendshipRequest);
        }

        var friendRequest = new FriendRequest(Id, friendId);

        Raise(new FriendRequestSentDomainEvent(friendRequest.Id));

        return friendRequest;
    }
}
