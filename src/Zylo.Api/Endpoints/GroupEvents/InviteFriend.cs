using MediatR;
using SharedKernel;
using Zylo.Api.Extensions;
using Zylo.Api.Infrastructure;
using Zylo.Application.GroupEvents.InviteFriend;
using Zylo.Contracts.GroupEvents;

namespace Zylo.Api.Endpoints.GroupEvents;

internal sealed class InviteFriend : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("group-events/{groupEventId}/invite", async (
            Guid groupEventId,
            InviteFriendToGroupEventRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new InviteFriendToGroupEventCommand(groupEventId, request.FriendId);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.GroupEvents)
        .RequireAuthorization();
    }
}
