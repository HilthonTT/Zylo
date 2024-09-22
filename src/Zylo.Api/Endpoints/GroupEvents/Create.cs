using MediatR;
using SharedKernel;
using Zylo.Api.Extensions;
using Zylo.Api.Infrastructure;
using Zylo.Application.GroupEvents.Create;
using Zylo.Contracts.GroupEvents;

namespace Zylo.Api.Endpoints.GroupEvents;

internal sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("group-events", async (
            CreateGroupEventRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateGroupEventCommand(request.UserId, request.Name, request.CategoryId, request.DateTime);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.GroupEvents)
        .RequireAuthorization();
    }
}
