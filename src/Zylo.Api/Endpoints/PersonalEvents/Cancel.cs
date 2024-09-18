using MediatR;
using SharedKernel;
using Zylo.Api.Extensions;
using Zylo.Api.Infrastructure;
using Zylo.Application.PersonalEvents.Cancel;

namespace Zylo.Api.Endpoints.PersonalEvents;

internal sealed class Cancel : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("personal-events/{personalEventId:guid}", async (
            Guid personalEventId,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new CancelPersonalEventCommand(personalEventId);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.PersonalEvents)
        .RequireAuthorization();
    }
}
