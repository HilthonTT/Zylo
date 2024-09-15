using MediatR;
using Microsoft.Extensions.Logging;
using System.Data;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Messaging;

namespace Zylo.Application.Abstractions.Behaviors;

internal sealed class TransactionalPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
{
    private readonly ILogger<TransactionalPipelineBehavior<TRequest, TResponse>> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public TransactionalPipelineBehavior(
        ILogger<TransactionalPipelineBehavior<TRequest, TResponse>> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Beginning transaction for {RequestName}", typeof(TRequest).Name);

        using IDbTransaction transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        TResponse response = await next();

        transaction.Commit();

        _logger.LogInformation("Committed transaction for {RequestName}", typeof(TRequest).Name);

        return response;
    }
}
