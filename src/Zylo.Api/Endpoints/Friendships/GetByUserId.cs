using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Zylo.Api.Extensions;
using Zylo.Api.Infrastructure;
using Zylo.Application.Friendships.GetByUserId;
using Zylo.Contracts.Common;
using Zylo.Contracts.Friendships;

namespace Zylo.Api.Endpoints.Friendships;

internal sealed class GetByUserId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("friendships/{userId:guid}", async (
            Guid userId,
            [FromQuery] int page,
            [FromQuery] int pageSize,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetFriendshipByUserIdQuery(userId, page, pageSize);

            Result<PagedList<FriendshipResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Friendships)
        .RequireAuthorization();
    }
}
