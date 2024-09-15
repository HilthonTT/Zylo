using SharedKernel;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Application.Abstractions.Notifications;
using Zylo.Application.Users.ResendEmail;
using Zylo.Domain.Users;
using Zylo.Domain.Users.ValueObjects;

namespace Zylo.Application.Users.ResendEmailVerification;

internal sealed class ResendUserEmailVerificationCommandHandler : ICommandHandler<ResendUserEmailVerificationCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailVerificationCodeRepository _emailVerificationCodeRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;

    public ResendUserEmailVerificationCommandHandler(
        IUserRepository userRepository,
        IEmailVerificationCodeRepository emailVerificationCodeRepository,
        IDateTimeProvider dateTimeProvider,
        IUnitOfWork unitOfWork,
        INotificationService notificationService)
    {
        _userRepository = userRepository;
        _emailVerificationCodeRepository = emailVerificationCodeRepository;
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
    }

    public async Task<Result> Handle(ResendUserEmailVerificationCommand request, CancellationToken cancellationToken)
    {
        Result<Email> emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure)
        {
            return Result.Failure(emailResult.Error);
        }

        User? user = await _userRepository.GetByEmailAsync(emailResult.Value, cancellationToken);
        if (user is null)
        {
            return Result.Failure(UserErrors.NotFoundByEmail);
        }

        var verificationCode = EmailVerificationCode.Create(user.Id, _dateTimeProvider.UtcNow);

        _emailVerificationCodeRepository.Insert(verificationCode);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _notificationService.SendAsync(
            user,
            "Email verification for Zylo",
            $"Here's your verification code: {verificationCode.Code}",
            cancellationToken);

        return Result.Success();
    }
}
