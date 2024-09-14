namespace Zylo.Domain.Users;

public interface IEmailVerificationCodeRepository
{
    Task<EmailVerificationCode?> GetByCodeAsync(int code, DateTime utcNow, CancellationToken cancellationToken = default);

    Task<EmailVerificationCode?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    void Insert(EmailVerificationCode emailVerificationCode);

    void Remove(EmailVerificationCode emailVerificationCode);
}
