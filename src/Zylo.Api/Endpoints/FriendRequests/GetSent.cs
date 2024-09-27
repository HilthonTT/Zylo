using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Zylo.Api.Extensions;
using Zylo.Api.Infrastructure;
using Zylo.Application.FriendRequests.GetSent;
using Zylo.Contracts.FriendRequests;

namespace Zylo.Api.Endpoints.FriendRequests;

internal sealed class GetSent : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("friend-requests/sent", async (
            [FromQuery] Guid userId,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetSentFriendRequestQuery(userId);

            Result<List<SentFriendRequestResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.FriendRequests)
        .RequireAuthorization();
    }
}
