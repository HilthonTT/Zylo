using SharedKernel;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Domain.Events.DomainEvents;
using Zylo.Domain.Notifications;

namespace Zylo.Application.PersonalEvents.Cancel;

internal sealed class PersonalEventCancelledDomainEventHandler : IDomainEventHandler<PersonalEventCancelledDomainEvent>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public PersonalEventCancelledDomainEventHandler(
        INotificationRepository notificationRepository,
        IDateTimeProvider dateTimeProvider)
    {
        _notificationRepository = notificationRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public Task Handle(PersonalEventCancelledDomainEvent notification, CancellationToken cancellationToken)
    {
        return _notificationRepository.RemoveNotificationsForEventAsync(
            notification.PersonalEventId,
            _dateTimeProvider.UtcNow, 
            cancellationToken);
    }
}
