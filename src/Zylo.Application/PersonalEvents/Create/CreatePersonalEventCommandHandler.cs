using SharedKernel;
using Zylo.Application.Abstractions.Authentication;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Domain.Events;
using Zylo.Domain.Events.ValueObjects;
using Zylo.Domain.Users;

namespace Zylo.Application.PersonalEvents.Create;

internal sealed class CreatePersonalEventCommandHandler : ICommandHandler<CreatePersonalEventCommand, Guid>
{
    private readonly IUserContext _userContext;
    private readonly IUserRepository _userRepository;
    private readonly IPersonalEventRepository _personalEventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreatePersonalEventCommandHandler(
        IUserContext userContext,
        IUserRepository userRepository,
        IPersonalEventRepository personalEventRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _userContext = userContext;
        _userRepository = userRepository;
        _personalEventRepository = personalEventRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<Guid>> Handle(CreatePersonalEventCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId != _userContext.UserId)
        {
            return Result.Failure<Guid>(UserErrors.InvalidPermissions);
        }

        DateTime dateTimeUtc = request.DateTime.ToUniversalTime();

        if (dateTimeUtc <= _dateTimeProvider.UtcNow)
        {
            return Result.Failure<Guid>(PersonalEventErrors.DateAndTimeIsInThePast);
        }

        User? user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound(request.UserId));
        }

        Category? category = Category.FromId(request.CategoryId);
        if (category is null)
        {
            return Result.Failure<Guid>(CategoryErrors.NotFound(request.CategoryId));
        }

        Result<Name> nameResult = Name.Create(request.Name);
        if (nameResult.IsFailure)
        {
            return Result.Failure<Guid>(nameResult.Error);
        }

        PersonalEvent personalEvent = user.CreatePersonalEvent(nameResult.Value, category, dateTimeUtc);

        _personalEventRepository.Insert(personalEvent);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return personalEvent.Id;
    }
}
