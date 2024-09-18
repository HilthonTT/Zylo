using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Zylo.Application.Abstractions.Authentication;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Contracts.Common;
using Zylo.Contracts.PersonalEvents;
using Zylo.Domain.Events;
using Zylo.Domain.Users;

namespace Zylo.Application.PersonalEvents.Get;

internal sealed class GetPersonalEventsQueryHandler : IQueryHandler<GetPersonalEventsQuery, PagedList<PersonalEventResponse>>
{
    private readonly IDbContext _context;
    private readonly IUserContext _userContext;

    public GetPersonalEventsQueryHandler(IDbContext context, IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task<Result<PagedList<PersonalEventResponse>>> Handle(
        GetPersonalEventsQuery request, 
        CancellationToken cancellationToken)
    {
        if (request.UserId != _userContext.UserId)
        {
            return Result.Failure<PagedList<PersonalEventResponse>>(UserErrors.InvalidPermissions);
        }

        bool shouldSearchCategory = request.CategoryId.HasValue && Category.Contains(request.CategoryId.Value);

        IQueryable<PersonalEventResponse> personalEventQuery = _context.PersonalEvents
            .AsNoTracking()
            .Where(personalEvent => personalEvent.UserId == request.UserId && !personalEvent.Cancelled)
            .Where(personalEvent =>
                // If a category filter is specified, match the category.
                (!shouldSearchCategory || personalEvent.Category.Id == request.CategoryId) &&
                // Filter by event name if specified.
                (string.IsNullOrWhiteSpace(request.Name) || personalEvent.Name.Value.Contains(request.Name)) &&
                // Filter by start date if specified.
                (request.StartDate == null || personalEvent.DateTimeUtc >= request.StartDate) &&
                // Filter by end date if specified.
                (request.EndDate == null || personalEvent.DateTimeUtc <= request.EndDate))
            .OrderByDescending(personalEvent => personalEvent.DateTimeUtc)
            .Select(personalEvent => new PersonalEventResponse(
                personalEvent.Id,
                personalEvent.Name,
                personalEvent.Category.Id,
                personalEvent.Category.Name,
                personalEvent.DateTimeUtc,
                personalEvent.CreatedOnUtc));

        PagedList<PersonalEventResponse> response = await PagedList<PersonalEventResponse>.CreateAsync(
            personalEventQuery,
            request.Page,
            request.PageSize,
            cancellationToken);

        return response;
    }
}
