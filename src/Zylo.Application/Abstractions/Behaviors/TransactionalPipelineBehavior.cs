using MediatR;
using Microsoft.Extensions.Logging;
using System.Data;
using Zylo.Application.Abstractions.Messaging;

namespace Zylo.Application.Abstractions.Behaviors;

internal sealed class TransactionalPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
{
    private readonly ILogger<TransactionalPipelineBehavior<TRequest, TResponse>> _logger;

    public TransactionalPipelineBehavior(ILogger<TransactionalPipelineBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Beginning transaction for {RequestName}", typeof(TRequest).Name);

        using IDbTransaction transaction = default!;

        TResponse response = await next();

        transaction.Commit();

        _logger.LogInformation("Committed transaction for {RequestName}", typeof(TRequest).Name);

        return response;
    }
}
