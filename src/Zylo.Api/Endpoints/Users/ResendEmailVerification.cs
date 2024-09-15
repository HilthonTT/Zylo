using MediatR;
using SharedKernel;
using Zylo.Api.Extensions;
using Zylo.Api.Infrastructure;
using Zylo.Application.Users.ResendEmail;
using Zylo.Contracts.Authentication;

namespace Zylo.Api.Endpoints.Users;

internal sealed class ResendEmailVerification : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/resend-email-verification", async (
            ResendEmailVerificationRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new ResendUserEmailVerificationCommand(request.Email);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Users);
    }
}
