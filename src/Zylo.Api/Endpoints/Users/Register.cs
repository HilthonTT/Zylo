using MediatR;
using SharedKernel;
using Zylo.Api.Extensions;
using Zylo.Api.Infrastructure;
using Zylo.Application.Users.Create;
using Zylo.Contracts.Authentication;

namespace Zylo.Api.Endpoints.Users;

internal sealed class Register : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/register", async (
            RegisterRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateUserCommand(request.FirstName, request.LastName, request.Email, request.Password);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Users);
    }
}
