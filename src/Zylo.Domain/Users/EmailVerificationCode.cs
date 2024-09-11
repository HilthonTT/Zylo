using SharedKernel;

namespace Zylo.Domain.Users;

public sealed class EmailVerificationCode : Entity, IAuditable
{
    public EmailVerificationCode(Guid id, Guid userId, int code, DateTime expiresOnUtc)
        : base(id)
    {
        Ensure.NotNullOrEmpty(userId, nameof(userId));
        Ensure.NotNull(expiresOnUtc, nameof(expiresOnUtc));
        Ensure.GreaterThanZero(code, nameof(code));

        UserId = userId;
        Code = code;
        ExpiresOnUtc = expiresOnUtc;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailVerificationCode"/> class.
    /// </summary>
    /// <remarks>
    /// Required by EF Core.
    /// </remarks>
    public EmailVerificationCode()
    {
    }

    public Guid UserId { get; private set; }

    public int Code { get; private set; }

    public DateTime ExpiresOnUtc { get; private set; }

    public DateTime CreatedOnUtc { get; set; }

    public DateTime? ModifiedOnUtc { get; set; }

    public User User { get; private set; }

    public void ExpireToken()
    {
        ExpiresOnUtc = DateTime.UtcNow;
    }
}
