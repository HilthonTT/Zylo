using MediatR;
using SharedKernel;
using Zylo.Api.Extensions;
using Zylo.Api.Infrastructure;
using Zylo.Application.Users.SendFriendRequest;
using Zylo.Contracts.Users;

namespace Zylo.Api.Endpoints.Users;

internal sealed class SendFriendRequest : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/{userId:guid}/send-friend-request", async (
            Guid userId,
            SendFriendRequestRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            if (userId != request.UserId)
            {
                return CustomResults.Problem(Error.UnProcessableRequest);
            }

            var command = new SendFriendRequestCommand(request.UserId, request.FriendId);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Users)
        .RequireAuthorization();
    }
}
