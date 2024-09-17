using SharedKernel;
using System.Data;
using Zylo.Application.Abstractions.Authentication;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Contracts.Users;
using Zylo.Domain.Users;
using Dapper;

namespace Zylo.Application.Users.GetById;

internal sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    private readonly IUserContext _userContext;
    private readonly IDbConnectionFactory _factory;

    public GetUserByIdQueryHandler(
        IUserContext userContext,
        IDbConnectionFactory factory)
    {
        _userContext = userContext;
        _factory = factory;
    }

    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.UserId != _userContext.UserId)
        {
            return Result.Failure<UserResponse>(UserErrors.InvalidPermissions);
        }

        const string sql =
            """
            SELECT 
                u.id AS Id,
                u.first_name AS FirstName,
                u.last_name AS LastName,
                u.created_on_utc AS CreatedOnUtc,
                COUNT(DISTINCT e.id) AS NumberOfPersonalEvents,
                COUNT(DISTINCT f.friend_id) AS NumberOfFriends
            FROM users u
            LEFT JOIN events e ON e.user_id = u.id AND e.cancelled = false
            LEFT JOIN friendships f ON f.user_id = u.id
            WHERE u.id = @UserId
            GROUP BY u.id, u.first_name, u.last_name, u.created_on_utc;
            """;

        using IDbConnection connection = await _factory.GetOpenConnectionAsync(cancellationToken);

        UserResponse? user = await connection.QueryFirstOrDefaultAsync<UserResponse>(
            sql,
            new { UserId = request.UserId });

        if (user is null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFound(request.UserId));
        }

        return user;
    }
}
