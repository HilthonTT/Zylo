using MediatR;
using SharedKernel;
using Zylo.Api.Extensions;
using Zylo.Api.Infrastructure;
using Zylo.Application.Friendships.Remove;

namespace Zylo.Api.Endpoints.Friendships;

internal sealed class Remove : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("friendships/{userId:guid}/{friendId:guid}", async (
            Guid userId,
            Guid friendId,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new RemoveFriendshipCommand(userId, friendId);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Friendships)
        .RequireAuthorization();
    }
}
