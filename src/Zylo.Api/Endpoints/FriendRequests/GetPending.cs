using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Zylo.Api.Extensions;
using Zylo.Api.Infrastructure;
using Zylo.Application.FriendRequests.GetPending;
using Zylo.Contracts.FriendRequests;

namespace Zylo.Api.Endpoints.FriendRequests;

internal sealed class GetPending : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("friend-requests/pending", async (
            [FromQuery] Guid userId,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetPendingFriendRequestsQuery(userId);

            Result<List<PendingFriendRequestResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.FriendRequests)
        .RequireAuthorization();
    }
}
