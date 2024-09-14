using Microsoft.EntityFrameworkCore;
using Zylo.Domain.Users;
using Zylo.Domain.Users.ValueObjects;

namespace Zylo.Persistence.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly ZyloDbContext _context;

    public UserRepository(ZyloDbContext context)
    {
        _context = context;
    }

    public Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.Email.Value == email.Value, cancellationToken);
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public void Insert(User user)
    {
        _context.Users.Add(user);
    }

    public async Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default)
    {
        return !await _context.Users.AnyAsync(u => u.Email.Value == email.Value, cancellationToken);
    }
}
