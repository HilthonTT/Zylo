using MediatR;
using SharedKernel;
using Zylo.Api.Extensions;
using Zylo.Api.Infrastructure;
using Zylo.Application.Users.Update;
using Zylo.Contracts.Users;

namespace Zylo.Api.Endpoints.Users;

internal sealed class Update : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("users/{userId:guid}", async (
            Guid userId,
            UpdateUserRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            if (userId != request.UserId)
            {
                return CustomResults.Problem(Error.UnProcessableRequest);
            }

            var command = new UpdateUserCommand(request.UserId, request.FirstName, request.LastName);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Users)
        .RequireAuthorization();
    }
}
