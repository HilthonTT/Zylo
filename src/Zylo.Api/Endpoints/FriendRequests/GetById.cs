using MediatR;
using SharedKernel;
using Zylo.Api.Extensions;
using Zylo.Api.Infrastructure;
using Zylo.Application.FriendRequests.GetById;
using Zylo.Contracts.FriendRequests;

namespace Zylo.Api.Endpoints.FriendRequests;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("friend-requests/{friendRequestId:guid}", async (
            Guid friendRequestId,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetFriendRequestByIdQuery(friendRequestId);

            Result<FriendRequestResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.FriendRequests)
        .RequireAuthorization();
    }
}
