using SharedKernel;
using Zylo.Application.Abstractions.Authentication;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Domain.Events;
using Zylo.Domain.Events.ValueObjects;
using Zylo.Domain.Users;

namespace Zylo.Application.PersonalEvents.Update;

internal sealed class UpdatePersonalEventCommandHandler : ICommandHandler<UpdatePersonalEventCommand>
{
    private readonly IUserContext _userContext;
    private readonly IPersonalEventRepository _personalEventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdatePersonalEventCommandHandler(
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

    public async Task<Result> Handle(UpdatePersonalEventCommand request, CancellationToken cancellationToken)
    {
        DateTime dateTimeUtc = request.DateTime.ToUniversalTime();

        if (dateTimeUtc <= _dateTimeProvider.UtcNow)
        {
            return Result.Failure(PersonalEventErrors.DateAndTimeIsInThePast);
        }

        PersonalEvent? personalEvent = await _personalEventRepository.GetByIdAsync(request.PersonalEventId, cancellationToken);
        if (personalEvent is null)
        {
            return Result.Failure(PersonalEventErrors.NotFound(request.PersonalEventId));
        }

        if (personalEvent.UserId != _userContext.UserId)
        {
            return Result.Failure(UserErrors.InvalidPermissions);
        }

        Result<Name> nameResult = Name.Create(request.Name);
        if (nameResult.IsFailure)
        {
            return Result.Failure(nameResult.Error);
        }

        personalEvent.ChangeName(nameResult.Value);

        personalEvent.ChangeDateAndTime(dateTimeUtc);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
