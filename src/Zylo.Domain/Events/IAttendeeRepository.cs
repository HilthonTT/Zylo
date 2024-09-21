using Zylo.Domain.Users;

namespace Zylo.Domain.Events;

public interface IAttendeeRepository
{
    Task<Attendee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<Attendee>> GetUnProcessedAsync(int take, CancellationToken cancellationToken = default);

    Task<(string Email, string Name)[]> GetEmailsAndNamesForGroupEvent(
        GroupEvent groupEvent, 
        CancellationToken cancellationToken = default);

    void Insert(Attendee attendee);

    Task MarkUnprocessedForGroupEventAsync(
        Guid groupEventId, 
        DateTime utcNow, 
        CancellationToken cancellationToken = default);

    Task RemoveAttendeesForGroupEventAsync(
        Guid groupEventId, 
        DateTime utcNow, 
        CancellationToken cancellationToken = default);

    Task<List<User>> GetUsersForGroupEventAsync(GroupEvent groupEvent, CancellationToken cancellationToken = default);
}
