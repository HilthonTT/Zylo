

namespace Zylo.Domain.Users;

public interface IEmailVerificationCodeRepository
{
    Task<EmailVerificationCode?> GetByCodeAsync(int code, DateTime utcNow, CancellationToken cancellationToken = default);

    Task<EmailVerificationCode?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<EmailVerificationCode>> GetInvalidOrExpiredCodesAsync(int batch, DateTime utcNow, CancellationToken cancellationToken = default);

    void Insert(EmailVerificationCode emailVerificationCode);

    void Remove(EmailVerificationCode emailVerificationCode);

    void RemoveRange(IEnumerable<EmailVerificationCode> emailVerificationCodes);
}
