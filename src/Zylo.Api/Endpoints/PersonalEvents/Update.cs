using MediatR;
using SharedKernel;
using Zylo.Api.Extensions;
using Zylo.Api.Infrastructure;
using Zylo.Application.PersonalEvents.Update;
using Zylo.Contracts.PersonalEvents;

namespace Zylo.Api.Endpoints.PersonalEvents;

internal sealed class Update : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("personal-events/{personalEventId:guid}", async (
            UpdatePersonalEventRequest request,
            Guid personalEventId,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdatePersonalEventCommand(personalEventId, request.Name, request.DateTime);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.PersonalEvents)
        .RequireAuthorization();
    }
}
