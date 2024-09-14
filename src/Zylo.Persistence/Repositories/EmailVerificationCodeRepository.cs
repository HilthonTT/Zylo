using Microsoft.EntityFrameworkCore;
using Zylo.Domain.Users;

namespace Zylo.Persistence.Repositories;

internal sealed class EmailVerificationCodeRepository : IEmailVerificationCodeRepository
{
    private readonly ZyloDbContext _context;

    public EmailVerificationCodeRepository(ZyloDbContext context)
    {
        _context = context;
    }

    public Task<EmailVerificationCode?> GetByCodeAsync(int code, DateTime utcNow, CancellationToken cancellationToken = default)
    {
        return _context.EmailVerificationCodes
            .Where(ev => ev.Code == code && ev.ExpiresOnUtc > utcNow)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<EmailVerificationCode?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.EmailVerificationCodes
            .FirstOrDefaultAsync(ev => ev.Id == id, cancellationToken);
    }

    public void Insert(EmailVerificationCode emailVerificationCode)
    {
        _context.EmailVerificationCodes.Add(emailVerificationCode);
    }

    public void Remove(EmailVerificationCode emailVerificationCode)
    {
        _context.EmailVerificationCodes.Remove(emailVerificationCode);
    }
}
