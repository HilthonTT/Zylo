using MediatR;
using SharedKernel;
using Zylo.Api.Extensions;
using Zylo.Api.Infrastructure;
using Zylo.Application.GroupEvents.Cancel;

namespace Zylo.Api.Endpoints.GroupEvents;

internal sealed class Cancel : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("group-events/{groupEventId:guid}", async (
            Guid groupEventId,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new CancelGroupEventCommand(groupEventId);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.GroupEvents)
        .RequireAuthorization();
    }
}
