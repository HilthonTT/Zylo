using SharedKernel;
using Zylo.Application.Abstractions.Events;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Domain.Events;
using Zylo.Domain.Events.DomainEvents;
using Zylo.Domain.Notifications;

namespace Zylo.Application.GroupEvents.DateAndTimeChanged;

internal sealed class GroupEventDateAndTimeChangedDomainEventHandler
    : IDomainEventHandler<GroupEventDateAndTimeChangedDomainEvent>
{
    private readonly IEventBus _eventBus;
    private readonly INotificationRepository _notificationRepository;
    private readonly IAttendeeRepository _attendeeRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public GroupEventDateAndTimeChangedDomainEventHandler(
        IEventBus eventBus,
        INotificationRepository notificationRepository,
        IAttendeeRepository attendeeRepository,
        IDateTimeProvider dateTimeProvider)
    {
        _eventBus = eventBus;
        _notificationRepository = notificationRepository;
        _attendeeRepository = attendeeRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task Handle(GroupEventDateAndTimeChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _eventBus.PublishAsync(
            new GroupEventDateAndTimeChangedIntegrationEvent(notification.GroupEventId), 
            cancellationToken);

        Task markAttendeesAsUnprocessed = MarkAttendeesAsUnprocessedAsync(notification.GroupEventId, cancellationToken);

        Task removeNotifications = RemoveNotificationsAsync(notification.GroupEventId, cancellationToken);

        await Task.WhenAll(markAttendeesAsUnprocessed, removeNotifications);
    }

    private Task MarkAttendeesAsUnprocessedAsync(Guid groupEventId, CancellationToken cancellationToken = default)
    {
        return _attendeeRepository.MarkUnprocessedForGroupEventAsync(groupEventId, _dateTimeProvider.UtcNow, cancellationToken);
    }

    private Task RemoveNotificationsAsync(Guid groupEventId, CancellationToken cancellationToken = default)
    {
        return _notificationRepository.RemoveNotificationsForEventAsync(groupEventId, _dateTimeProvider.UtcNow, cancellationToken);
    }
}
