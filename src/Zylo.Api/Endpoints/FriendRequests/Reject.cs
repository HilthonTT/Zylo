using MediatR;
using SharedKernel;
using Zylo.Api.Extensions;
using Zylo.Api.Infrastructure;
using Zylo.Application.FriendRequests.Reject;

namespace Zylo.Api.Endpoints.FriendRequests;

internal sealed class Reject : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("friend-requests/{friendRequestId:guid}", async (
            Guid friendRequestId,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new RejectFriendRequestCommand(friendRequestId);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.FriendRequests)
        .RequireAuthorization();
    }
}
