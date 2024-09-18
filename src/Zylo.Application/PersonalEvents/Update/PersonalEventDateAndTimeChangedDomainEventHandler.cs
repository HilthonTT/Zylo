using SharedKernel;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Domain.Events.DomainEvents;
using Zylo.Domain.Notifications;

namespace Zylo.Application.PersonalEvents.Update;

internal sealed class PersonalEventDateAndTimeChangedDomainEventHandler :
    IDomainEventHandler<PersonalEventDateAndTimeChangedDomainEvent>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public PersonalEventDateAndTimeChangedDomainEventHandler(
        INotificationRepository notificationRepository,
        IDateTimeProvider dateTimeProvider)
    {
        _notificationRepository = notificationRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public Task Handle(PersonalEventDateAndTimeChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        return _notificationRepository.RemoveNotificationsForEventAsync(
            notification.PersonalEventId, 
            _dateTimeProvider.UtcNow, 
            cancellationToken);
    }
}
