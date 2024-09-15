using MediatR;
using SharedKernel;
using Zylo.Api.Extensions;
using Zylo.Api.Infrastructure;
using Zylo.Application.Users.Verify;
using Zylo.Contracts.Authentication;

namespace Zylo.Api.Endpoints.Users;

internal sealed class VerifyEmail : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/verify-email", async (
            VerifyUserRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new VerifyUserCommand(request.Code);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Users);
    }
}
