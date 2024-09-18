namespace Zylo.Domain.Events;

public interface IPersonalEventRepository
{
    Task<PersonalEvent?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<PersonalEvent>> GetUnProcessedAsync(int take, CancellationToken cancellationToken = default);

    void Insert(PersonalEvent personalEvent);
}
