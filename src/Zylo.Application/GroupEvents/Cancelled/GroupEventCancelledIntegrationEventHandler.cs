using SharedKernel;
using System.Globalization;
using Zylo.Application.Abstractions.Events;
using Zylo.Application.Abstractions.Notifications;
using Zylo.Domain.Events;
using Zylo.Domain.Users;

namespace Zylo.Application.GroupEvents.Cancelled;

internal sealed class GroupEventCancelledIntegrationEventHandler : IIntegrationEventHandler<GroupEventCancelledIntegrationEvent>
{
    private readonly IGroupEventRepository _groupEventRepository;
    private readonly IAttendeeRepository _attendeeRepository;
    private readonly INotificationService _notificationService;

    public GroupEventCancelledIntegrationEventHandler(
        IGroupEventRepository groupEventRepository,
        IAttendeeRepository attendeeRepository,
        INotificationService notificationService)
    {
        _groupEventRepository = groupEventRepository;
        _attendeeRepository = attendeeRepository;
        _notificationService = notificationService;
    }

    public async Task Handle(GroupEventCancelledIntegrationEvent notification, CancellationToken cancellationToken)
    {
        GroupEvent? groupEvent = await _groupEventRepository.GetByIdAsync(notification.Id, cancellationToken);
        if (groupEvent is null)
        {
            throw new DomainException(GroupEventErrors.NotFound(notification.Id));
        }

        List<User> users = await _attendeeRepository.GetUsersForGroupEventAsync(groupEvent, cancellationToken);

        if (users.Count == 0)
        {
            return;
        }

        IEnumerable<Task> sendGroupEventCancelledEmailTasks = users
               .Select(user => _notificationService.SendAsync(
                   user, 
                   $"{groupEvent.Name} has been cancelled 😞",
                   $"Hello {groupEvent.Name}," +
                   Environment.NewLine +
                   Environment.NewLine +
                   $"Unfortunately, the event {groupEvent.Name} " +
                   $"({groupEvent.DateTimeUtc.ToString(CultureInfo.InvariantCulture)}) has been cancelled."));

        await Task.WhenAll(sendGroupEventCancelledEmailTasks);
    }
}
