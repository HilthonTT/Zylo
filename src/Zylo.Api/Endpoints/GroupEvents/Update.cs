using MediatR;
using SharedKernel;
using Zylo.Api.Extensions;
using Zylo.Api.Infrastructure;
using Zylo.Application.GroupEvents.Update;
using Zylo.Contracts.GroupEvents;

namespace Zylo.Api.Endpoints.GroupEvents;

internal sealed class Update : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("group-events/{groupEventId:guid}", async (
            Guid groupEventId,
            UpdateGroupEventRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateGroupEventCommand(groupEventId, request.Name, request.DateTime);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.GroupEvents)
        .RequireAuthorization();
    }
}
