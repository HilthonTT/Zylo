using MediatR;
using SharedKernel;
using Zylo.Api.Extensions;
using Zylo.Api.Infrastructure;
using Zylo.Application.FriendRequests.Accept;

namespace Zylo.Api.Endpoints.FriendRequests;

internal sealed class Accept : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("friend-requests/{friendRequestId:guid}/accept", async (
            Guid friendRequestId,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new AcceptFriendRequestCommand(friendRequestId);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.FriendRequests)
        .RequireAuthorization();
    }
}
