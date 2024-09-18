using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Zylo.Api.Extensions;
using Zylo.Api.Infrastructure;
using Zylo.Application.PersonalEvents.Get;
using Zylo.Contracts.Common;
using Zylo.Contracts.PersonalEvents;

namespace Zylo.Api.Endpoints.PersonalEvents;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("personal-events", async (
            [FromQuery] Guid userId,
            [FromQuery] string? name,
            [FromQuery] int categoryId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int page,
            [FromQuery] int pageSize,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetPersonalEventsQuery(userId, name, categoryId, startDate, endDate, page, pageSize);

            Result<PagedList<PersonalEventResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.PersonalEvents)
        .RequireAuthorization();
    }
}
