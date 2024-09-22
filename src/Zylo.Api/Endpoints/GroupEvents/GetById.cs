using MediatR;
using SharedKernel;
using Zylo.Api.Extensions;
using Zylo.Api.Infrastructure;
using Zylo.Application.GroupEvents.GetById;
using Zylo.Contracts.GroupEvents;

namespace Zylo.Api.Endpoints.GroupEvents;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("group-events/{groupEventId:guid}", async (
            Guid groupEventId,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetGroupEventByIdQuery(groupEventId);

            Result<DetailedGroupEventResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.GroupEvents)
        .RequireAuthorization();
    }
}
