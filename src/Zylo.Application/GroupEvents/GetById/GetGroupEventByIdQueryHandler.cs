using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Zylo.Application.Abstractions.Authentication;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Contracts.GroupEvents;
using Zylo.Domain.Events;
using Zylo.Domain.Users;

namespace Zylo.Application.GroupEvents.GetById;

internal sealed class GetGroupEventByIdQueryHandler : IQueryHandler<GetGroupEventByIdQuery, DetailedGroupEventResponse>
{
    private readonly IDbContext _dbContext;
    private readonly IUserContext _userContext;

    public GetGroupEventByIdQueryHandler(IDbContext dbContext, IUserContext userContext)
    {
        _dbContext = dbContext;
        _userContext = userContext;
    }

    public async Task<Result<DetailedGroupEventResponse>> Handle(
        GetGroupEventByIdQuery request, 
        CancellationToken cancellationToken)
    {
        if (!await HasPermissionsToQueryAsync(request.GroupEventId, cancellationToken))
        {
            return Result.Failure<DetailedGroupEventResponse>(UserErrors.InvalidPermissions);
        }

        DetailedGroupEventResponse? response = await _dbContext.GroupEvents
            .AsNoTracking()
            .Join(
                _dbContext.Users.AsNoTracking(),
                groupEvent => groupEvent.UserId,
                user => user.Id,
                (groupEvent, user) => new { groupEvent, user }
            )
            .Where(result => result.groupEvent.Id == request.GroupEventId && !result.groupEvent.Cancelled)
            .Select(result => new DetailedGroupEventResponse(
                result.groupEvent.Id,
                result.groupEvent.Name.Value,
                result.groupEvent.Category.Id,
                result.groupEvent.Category.Name,
                result.user.FullName,
                0,
                result.groupEvent.DateTimeUtc,
                result.groupEvent.CreatedOnUtc))
            .FirstOrDefaultAsync(cancellationToken);

        if (response is null)
        {
            return Result.Failure<DetailedGroupEventResponse>(GroupEventErrors.NotFound(request.GroupEventId));
        }

        int attendeesCount = await _dbContext.Attendees
            .Where(x => x.EventId == response.Id)
            .CountAsync(cancellationToken);

        response = response with { NumberOfAttendees = attendeesCount };

        return response;
    }

    private async Task<bool> HasPermissionsToQueryAsync(Guid groupEventId, CancellationToken cancellationToken) =>
         await _dbContext.Attendees
            .AsNoTracking()
            .Join(
                _dbContext.GroupEvents.AsNoTracking(),
                attendee => attendee.EventId,
                groupEvent => groupEvent.Id,
                (attendee, groupEvent) => new { attendee, groupEvent }
            )
            .Where(result => result.groupEvent.Id == groupEventId &&
                             !result.groupEvent.Cancelled &&
                             (result.groupEvent.UserId == _userContext.UserId ||
                              result.attendee.UserId == _userContext.UserId))
            .Select(result => true)
            .AnyAsync(cancellationToken);
}
