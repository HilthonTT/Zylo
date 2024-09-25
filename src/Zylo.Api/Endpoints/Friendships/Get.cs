using MediatR;
using SharedKernel;
using Zylo.Api.Extensions;
using Zylo.Api.Infrastructure;
using Zylo.Application.Friendships.Get;
using Zylo.Contracts.Friendships;

namespace Zylo.Api.Endpoints.Friendships;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("friendships/{userId:guid}/{friendId:guid}", async (
            Guid userId,
            Guid friendId,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetFriendshipQuery(userId, friendId);

            Result<FriendshipResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Friendships)
        .RequireAuthorization();
    }
}
