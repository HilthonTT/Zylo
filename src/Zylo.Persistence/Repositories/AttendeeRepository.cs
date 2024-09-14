using Microsoft.EntityFrameworkCore;
using Zylo.Domain.Events;

namespace Zylo.Persistence.Repositories;

internal sealed class AttendeeRepository : IAttendeeRepository
{
    private readonly ZyloDbContext _context;

    public AttendeeRepository(ZyloDbContext context)
    {
        _context = context;
    }

    public Task<Attendee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Attendees.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<(string Email, string Name)[]> GetEmailsAndNamesForGroupEvent(
        GroupEvent groupEvent, 
        CancellationToken cancellationToken = default)
    {
        var attendeeEmailsAndName = await _context
            .GroupEvents
            .Where(g => g.Id == groupEvent.Id)
            .Join(_context.Attendees,
                @event => @event.Id,
                attendee => attendee.EventId,
                (@event, attendee) => new { @event, attendee })
            .Join(_context.Users,
                eventAttendee => eventAttendee.attendee.UserId,
                user => user.Id,
                (eventAttendee, user) => new
                {
                    eventAttendee.@event,
                    eventAttendee.attendee,
                    UserEmail = user.Email.Value,
                    UserName = user.FullName,
                })
            .Where(result => result.attendee.UserId != groupEvent.UserId)
            .Select(result => new
            {
                Email = result.UserEmail,
                Name = result.UserName,
            })
            .ToListAsync(cancellationToken);

        return attendeeEmailsAndName.Select(x => (x.Email, x.Name)).ToArray();
    }

    public Task<List<Attendee>> GetUnProcessedAsync(
        int take, 
        CancellationToken cancellationToken = default)
    {
        return _context.Attendees
            .Where(a => !a.Processed)
            .OrderBy(a => a.CreatedOnUtc)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public void Insert(Attendee attendee)
    {
        _context.Attendees.Add(attendee);
    }

    public Task MarkUnprocessedForGroupEventAsync(
        GroupEvent groupEvent, 
        DateTime utcNow, 
        CancellationToken cancellationToken = default)
    {
        return _context.Attendees
            .Where(a => a.EventId == groupEvent.Id && !a.IsDeleted)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(a => a.Processed, false)
                .SetProperty(a => a.ModifiedOnUtc, utcNow),
                cancellationToken);
    }

    public Task RemoveAttendeesForGroupEventAsync(
        GroupEvent groupEvent, 
        DateTime utcNow, 
        CancellationToken cancellationToken = default)
    {
        return _context.Attendees
            .Where(a => a.EventId == groupEvent.Id && !a.IsDeleted)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(a => a.IsDeleted, true)
                .SetProperty(a => a.DeletedOnUtc, utcNow),
                cancellationToken);
    }
}
