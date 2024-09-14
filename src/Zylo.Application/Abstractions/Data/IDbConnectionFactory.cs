using System.Data;

namespace Zylo.Application.Abstractions.Data;

public interface IDbConnectionFactory
{
    Task<IDbConnection> GetOpenConnectionAsync(CancellationToken cancellationToken = default);

    IDbConnection GetOpenConnection();
}
