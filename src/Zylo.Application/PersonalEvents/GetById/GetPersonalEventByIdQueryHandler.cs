using Dapper;
using SharedKernel;
using System.Data;
using Zylo.Application.Abstractions.Authentication;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Contracts.PersonalEvents;
using Zylo.Domain.Events;

namespace Zylo.Application.PersonalEvents.GetById;

internal sealed class GetPersonalEventByIdQueryHandler : IQueryHandler<GetPersonalEventByIdQuery, DetailedPersonalEventResponse>
{
    private readonly IUserContext _userContext;
    private readonly IDbConnectionFactory _factory;

    public GetPersonalEventByIdQueryHandler(IUserContext userContext, IDbConnectionFactory factory)
    {
        _userContext = userContext;
        _factory = factory;
    }

    public async Task<Result<DetailedPersonalEventResponse>> Handle(
        GetPersonalEventByIdQuery request, 
        CancellationToken cancellationToken)
    {
        const string sql =
            """
            SELECT 
                pe.id AS Id,
                pe.name AS Name,
                pe.category AS CategoryId,
                pe.category_name AS Category,
                CONCAT(u.first_name, ' ', u.last_name) AS CreatedBy,
                pe.date_time_utc AS DateTimeUtc,
                pe.created_on_utc AS CreatedOnUtc
            FROM 
                events pe
            JOIN 
                users u ON pe.user_id = u.id
            WHERE 
                pe.id = @PersonalEventId
                AND pe.is_deleted = false
            LIMIT 1;
            """;

        using IDbConnection connection = await _factory.GetOpenConnectionAsync(cancellationToken);

        DetailedPersonalEventResponse? response = await connection.QueryFirstOrDefaultAsync<DetailedPersonalEventResponse>(
            sql,
            new { PersonalEventId = request.PersonalEventId });

        if (response is null)
        {
            return Result.Failure<DetailedPersonalEventResponse>(PersonalEventErrors.NotFound(request.PersonalEventId));
        }

        return response;
    }
}
