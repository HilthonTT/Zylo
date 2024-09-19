using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Zylo.Application.Abstractions.Authentication;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Contracts.GroupEvents;
using Zylo.Domain.Users;

namespace Zylo.Application.GroupEvents.GetTenMostRecentAttending;

internal sealed class GetTenMostRecentAttendingGroupEventsQueryHandler :
    IQueryHandler<GetTenMostRecentAttendingGroupEventsQuery, List<GroupEventResponse>>
{
    private readonly IUserContext _userContext;
    private readonly IDbContext _dbContext;

    public GetTenMostRecentAttendingGroupEventsQueryHandler(
        IUserContext userContext,
        IDbContext dbContext)
    {
        _userContext = userContext;
        _dbContext = dbContext;
    }

    public async Task<Result<List<GroupEventResponse>>> Handle(
        GetTenMostRecentAttendingGroupEventsQuery request, 
        CancellationToken cancellationToken)
    {
        if (request.UserId != _userContext.UserId)
        {
            return Result.Failure<List<GroupEventResponse>>(UserErrors.InvalidPermissions);
        }

        List<GroupEventResponse> groupEvents = await _dbContext.Attendees
            .AsNoTracking()
            .Where(a => a.UserId == request.UserId)
            .Join(
                _dbContext.GroupEvents.AsNoTracking(),
                attendee => attendee.EventId,
                groupEvent => groupEvent.Id,
                (attendee, groupEvent) => new GroupEventResponse
                (
                    groupEvent.Id,
                    groupEvent.Name.Value,
                    groupEvent.Category.Id,
                    groupEvent.Category.Name,
                    groupEvent.DateTimeUtc,
                    groupEvent.CreatedOnUtc))
            .OrderBy(ge => ge.DateTimeUtc)
            .Take(request.NumberOfGroupEventsToTake)
            .ToListAsync(cancellationToken);

        return groupEvents;
    }
}
