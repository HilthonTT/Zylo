using SharedKernel;
using Zylo.Application.Abstractions.Authentication;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Domain.Events;
using Zylo.Domain.Users;

namespace Zylo.Application.PersonalEvents.Cancel;

internal sealed class CancelPersonalEventCommandHandler : ICommandHandler<CancelPersonalEventCommand>
{
    private readonly IUserContext _userContext;
    private readonly IPersonalEventRepository _personalEventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CancelPersonalEventCommandHandler(
        IUserContext userContext,
        IPersonalEventRepository personalEventRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _userContext = userContext;
        _personalEventRepository = personalEventRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result> Handle(CancelPersonalEventCommand request, CancellationToken cancellationToken)
    {
        PersonalEvent? personalEvent = await _personalEventRepository.GetByIdAsync(request.PersonalEventId, cancellationToken);

        if (personalEvent is null)
        {
            return Result.Failure(PersonalEventErrors.NotFound(request.PersonalEventId));
        }

        if (personalEvent.UserId != _userContext.UserId)
        {
            return Result.Failure(UserErrors.InvalidPermissions);
        }

        Result result = personalEvent.Cancel(_dateTimeProvider.UtcNow);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }
}
