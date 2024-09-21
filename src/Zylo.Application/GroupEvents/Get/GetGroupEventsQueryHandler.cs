using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Zylo.Application.Abstractions.Authentication;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Contracts.Common;
using Zylo.Contracts.GroupEvents;
using Zylo.Domain.Events;
using Zylo.Domain.Users;

namespace Zylo.Application.GroupEvents.Get;

internal sealed class GetGroupEventsQueryHandler : IQueryHandler<GetGroupEventsQuery, PagedList<GroupEventResponse>>
{
    private readonly IDbContext _context;
    private readonly IUserContext _userContext;

    public GetGroupEventsQueryHandler(IDbContext context, IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task<Result<PagedList<GroupEventResponse>>> Handle(
        GetGroupEventsQuery request, 
        CancellationToken cancellationToken)
    {
        if (request.UserId != _userContext.UserId)
        {
            return Result.Failure<PagedList<GroupEventResponse>>(UserErrors.InvalidPermissions);
        }

        bool shouldSearchCategory = request.CategoryId.HasValue && Category.Contains(request.CategoryId.Value);

        IQueryable<GroupEventResponse> query =
            _context.GroupEvents
                .AsNoTracking()
                .Join(
                    _context.Users.AsNoTracking(),
                    groupEvent => groupEvent.UserId,
                    user => user.Id,
                    (groupEvent, user) => new { groupEvent, user }
                )
                .Where(result => 
                    result.groupEvent.UserId == request.UserId &&
                    !result.groupEvent.Cancelled &&
                    (!shouldSearchCategory || result.groupEvent.Category.Id == request.CategoryId) &&
                    (string.IsNullOrWhiteSpace(request.Name) ||
                    result.groupEvent.Name.Value.Contains(request.Name)) &&
                    (request.StartDate == null ||
                    result.groupEvent.DateTimeUtc >= request.StartDate) &&
                    (request.EndDate == null ||
                    result.groupEvent.DateTimeUtc <= request.EndDate))
                .OrderByDescending(result => result.groupEvent.DateTimeUtc)
                .Select(result => new GroupEventResponse(
                    result.groupEvent.Id,
                    result.groupEvent.Name.Value,
                    result.groupEvent.Category.Id,
                    result.groupEvent.Category.Name,
                    result.groupEvent.DateTimeUtc,
                    result.groupEvent.CreatedOnUtc));

        var response = await PagedList<GroupEventResponse>.CreateAsync(
            query, 
            request.Page,
            request.PageSize, 
            cancellationToken);

        return response;
    }
}
