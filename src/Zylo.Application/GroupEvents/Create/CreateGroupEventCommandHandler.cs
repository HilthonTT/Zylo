using SharedKernel;
using Zylo.Application.Abstractions.Authentication;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Domain.Events;
using Zylo.Domain.Events.ValueObjects;
using Zylo.Domain.Users;

namespace Zylo.Application.GroupEvents.Create;

internal sealed class CreateGroupEventCommandHandler : ICommandHandler<CreateGroupEventCommand, Guid>
{
    private readonly IUserContext _userContext;
    private readonly IUserRepository _userRepository;
    private readonly IGroupEventRepository _groupEventRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUnitOfWork _unitOfWork;

    public CreateGroupEventCommandHandler(
        IUserContext userContext,
        IUserRepository userRepository,
        IGroupEventRepository groupEventRepository,
        IDateTimeProvider dateTimeProvider,
        IUnitOfWork unitOfWork)
    {
        _userContext = userContext;
        _userRepository = userRepository;
        _groupEventRepository = groupEventRepository;
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateGroupEventCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId != _userContext.UserId)
        {
            return Result.Failure<Guid>(UserErrors.InvalidPermissions);
        }

        DateTime dateTimeUtc = request.DateTime.ToUniversalTime();

        if (dateTimeUtc <= _dateTimeProvider.UtcNow)
        {
            return Result.Failure<Guid>(GroupEventErrors.DateAndTimeIsInThePast);
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

        var groupEvent = GroupEvent.Create(user.Id, nameResult.Value, category, dateTimeUtc);

        _groupEventRepository.Insert(groupEvent);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return groupEvent.Id;
    }
}
