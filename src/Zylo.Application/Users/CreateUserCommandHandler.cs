using SharedKernel;
using Zylo.Application.Abstractions.Authentication;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Application.Abstractions.Notifications;
using Zylo.Domain.Users;
using Zylo.Domain.Users.ValueObjects;

namespace Zylo.Application.Users;

internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid> 
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailVerificationCodeRepository _emailVerificationCodeRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly INotificationService _notificationService;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IEmailVerificationCodeRepository emailVerificationCodeRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        INotificationService notificationService)
    {
        _userRepository = userRepository;
        _emailVerificationCodeRepository = emailVerificationCodeRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _notificationService = notificationService;
    }

    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        Result<FirstName> firstNameResult = FirstName.Create(request.FirstName);
        Result<LastName> lastNameResult = LastName.Create(request.LastName);
        Result<Email> emailResult = Email.Create(request.Email);
        Result<Password> passwordResult = Password.Create(request.Password);

        Result firstFailureOrSuccess = Result.FirstFailureOrSuccess(firstNameResult, lastNameResult, emailResult, passwordResult);

        if (firstFailureOrSuccess.IsFailure)
        {
            return Result.Failure<Guid>(firstFailureOrSuccess.Error);
        }

        if (!await _userRepository.IsEmailUniqueAsync(emailResult.Value, cancellationToken))
        {
            return Result.Failure<Guid>(UserErrors.EmailNotUnique);
        }

        string passwordHash = _passwordHasher.Hash(passwordResult.Value);

        var user = User.Create(emailResult.Value, firstNameResult.Value, lastNameResult.Value, passwordHash);

        _userRepository.Insert(user);

        var verificationCode = EmailVerificationCode.Create(user.Id, _dateTimeProvider.UtcNow);

        _emailVerificationCodeRepository.Insert(verificationCode);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
