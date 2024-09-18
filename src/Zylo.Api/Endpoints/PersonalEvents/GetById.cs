using MediatR;
using SharedKernel;
using Zylo.Api.Extensions;
using Zylo.Api.Infrastructure;
using Zylo.Application.PersonalEvents.GetById;
using Zylo.Contracts.PersonalEvents;

namespace Zylo.Api.Endpoints.PersonalEvents;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("personal-events/{personalEventId:guid}", async (
            Guid personalEventId,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetPersonalEventByIdQuery(personalEventId);

            Result<DetailedPersonalEventResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.PersonalEvents)
        .RequireAuthorization();
    }
}
