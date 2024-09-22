using SharedKernel;
using Zylo.Application.Abstractions.Authentication;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Domain.Events;
using Zylo.Domain.Events.ValueObjects;
using Zylo.Domain.Users;

namespace Zylo.Application.GroupEvents.Update;

internal sealed class UpdateGroupEventCommandHandler : ICommandHandler<UpdateGroupEventCommand>
{
    private readonly IUserContext _userContext;
    private readonly IGroupEventRepository _groupEventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateGroupEventCommandHandler(
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

    public async Task<Result> Handle(UpdateGroupEventCommand request, CancellationToken cancellationToken)
    {
        DateTime dateTimeUtc = request.DateTime.ToUniversalTime();

        if (dateTimeUtc <= _dateTimeProvider.UtcNow)
        {
            return Result.Failure(GroupEventErrors.DateAndTimeIsInThePast);
        }

        GroupEvent? groupEvent = await _groupEventRepository.GetByIdAsync(request.GroupEventId, cancellationToken);

        if (groupEvent is null)
        {
            return Result.Failure(GroupEventErrors.NotFound(request.GroupEventId));
        }

        if (groupEvent.UserId != _userContext.UserId)
        {
            return Result.Failure(UserErrors.InvalidPermissions);
        }

        Result<Name> nameResult = Name.Create(request.Name);
        if (nameResult.IsFailure)
        {
            return Result.Failure(nameResult.Error);
        }

        groupEvent.ChangeName(nameResult.Value);

        groupEvent.ChangeDateAndTime(dateTimeUtc);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
