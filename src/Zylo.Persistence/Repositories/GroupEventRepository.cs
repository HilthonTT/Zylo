using Microsoft.EntityFrameworkCore;
using Zylo.Domain.Events;

namespace Zylo.Persistence.Repositories;

internal sealed class GroupEventRepository : IGroupEventRepository
{
    private readonly ZyloDbContext _context;

    public GroupEventRepository(ZyloDbContext context)
    {
        _context = context;
    }

    public Task<GroupEvent?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.GroupEvents.FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
    }

    public Task<List<GroupEvent>> GetForAttendeesAsync(
        List<Attendee> attendees, 
        CancellationToken cancellationToken = default)
    {
        Guid[] groupEventIds = attendees.Select(a => a.EventId).Distinct().ToArray();

        return _context.GroupEvents.Where(g => groupEventIds.Contains(g.Id)).ToListAsync(cancellationToken);
    }

    public void Insert(GroupEvent groupEvent)
    {
        _context.GroupEvents.Add(groupEvent);
    }
}
