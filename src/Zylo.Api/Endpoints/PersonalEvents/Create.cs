using MediatR;
using SharedKernel;
using Zylo.Api.Extensions;
using Zylo.Api.Infrastructure;
using Zylo.Application.PersonalEvents.Create;
using Zylo.Contracts.PersonalEvents;

namespace Zylo.Api.Endpoints.PersonalEvents;

internal sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("personal-events", async (
            CreatePersonalEventRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new CreatePersonalEventCommand(request.UserId, request.Name, request.CategoryId, request.DateTimeUtc);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
         .WithTags(Tags.PersonalEvents)
         .RequireAuthorization();
    }
}
