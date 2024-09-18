using Microsoft.EntityFrameworkCore;
using Zylo.Domain.Events;

namespace Zylo.Persistence.Repositories;

internal sealed class PersonalEventRepository : IPersonalEventRepository
{
    private readonly ZyloDbContext _context;

    public PersonalEventRepository(ZyloDbContext context)
    {
        _context = context;
    }

    public Task<PersonalEvent?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.PersonalEvents.FirstOrDefaultAsync(pe => pe.Id == id, cancellationToken);
    }

    public Task<List<PersonalEvent>> GetUnProcessedAsync(int take, CancellationToken cancellationToken = default)
    {
        return _context.PersonalEvents
            .Where(pe => !pe.Processed)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public void Insert(PersonalEvent personalEvent)
    {
        _context.PersonalEvents.Add(personalEvent);
    }
}
