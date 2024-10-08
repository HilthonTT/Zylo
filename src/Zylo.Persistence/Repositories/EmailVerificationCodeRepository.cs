﻿using Microsoft.EntityFrameworkCore;
using Zylo.Domain.Users;

namespace Zylo.Persistence.Repositories;

internal sealed class EmailVerificationCodeRepository : IEmailVerificationCodeRepository
{
    private readonly ZyloDbContext _context;

    public EmailVerificationCodeRepository(ZyloDbContext context)
    {
        _context = context;
    }

    public Task<List<EmailVerificationCode>> GetInvalidOrExpiredCodesAsync(
        int batch, 
        DateTime utcNow, 
        CancellationToken cancellationToken = default)
    {
        return _context.EmailVerificationCodes
           .Where(ev => ev.ExpiresOnUtc <= utcNow || ev.User.EmailVerified)
           .OrderBy(ev => ev.ExpiresOnUtc)
           .Take(batch)
           .Include(ev => ev.User)
           .AsSplitQuery()
           .ToListAsync(cancellationToken);
    }

    public Task<EmailVerificationCode?> GetByCodeAsync(int code, DateTime utcNow, CancellationToken cancellationToken = default)
    {
        return _context.EmailVerificationCodes
            .Where(ev => ev.Code == code && ev.ExpiresOnUtc > utcNow)
            .Include(ev => ev.User)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<EmailVerificationCode?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.EmailVerificationCodes
            .Include(ev => ev.User)
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

    public void RemoveRange(IEnumerable<EmailVerificationCode> emailVerificationCodes)
    {
        _context.EmailVerificationCodes.RemoveRange(emailVerificationCodes);
    }
}
