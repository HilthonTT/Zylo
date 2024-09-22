using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Zylo.Api.Extensions;
using Zylo.Api.Infrastructure;
using Zylo.Application.GroupEvents.Get;
using Zylo.Contracts.Common;
using Zylo.Contracts.GroupEvents;

namespace Zylo.Api.Endpoints.GroupEvents;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("group-events", async (
            [FromQuery] Guid userId,
            [FromQuery] int page,
            [FromQuery] int pageSize,
            [FromQuery] string? name,
            [FromQuery] int? categoryId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetGroupEventsQuery(userId, name, categoryId, startDate, endDate, page, pageSize);

            Result<PagedList<GroupEventResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.GroupEvents)
        .RequireAuthorization();
    }
}
