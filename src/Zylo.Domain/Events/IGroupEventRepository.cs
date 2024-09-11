namespace Zylo.Domain.Events;

public interface IGroupEventRepository
{
    Task<GroupEvent?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<GroupEvent>> GetForAttendeesAsync(List<Attendee> attendees, CancellationToken cancellationToken = default);

    void Insert(GroupEvent groupEvent);
}
