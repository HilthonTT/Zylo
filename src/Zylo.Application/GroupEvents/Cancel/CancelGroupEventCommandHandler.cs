using SharedKernel;
using Zylo.Application.Abstractions.Authentication;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Domain.Events;
using Zylo.Domain.Users;

namespace Zylo.Application.GroupEvents.Cancel;

internal sealed class CancelGroupEventCommandHandler : ICommandHandler<CancelGroupEventCommand>
{
    private readonly IUserContext _userContext;
    private readonly IGroupEventRepository _groupEventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CancelGroupEventCommandHandler(
        IUserContext userContext,
        IGroupEventRepository groupEventRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _userContext = userContext;
        _groupEventRepository = groupEventRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result> Handle(CancelGroupEventCommand request, CancellationToken cancellationToken)
    {
        GroupEvent? groupEvent = await _groupEventRepository.GetByIdAsync(request.GroupEventId, cancellationToken);
        if (groupEvent is null)
        {
            return Result.Failure(GroupEventErrors.NotFound(request.GroupEventId));
        }

        if (groupEvent.UserId != _userContext.UserId)
        {
            return Result.Failure(UserErrors.InvalidPermissions);
        }

        Result result = groupEvent.Cancel(_dateTimeProvider.UtcNow);

        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
