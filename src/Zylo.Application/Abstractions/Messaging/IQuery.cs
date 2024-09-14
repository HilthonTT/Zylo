using MediatR;
using SharedKernel;

namespace Zylo.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
