using SharedKernel;
using Zylo.Application.Abstractions.Authentication;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Contracts.Authentication;
using Zylo.Domain.Users;
using Zylo.Domain.Users.ValueObjects;

namespace Zylo.Application.Users.Login;

internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand, TokenResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenProvider _tokenProvider;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenProvider tokenProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenProvider = tokenProvider;
    }

    public async Task<Result<TokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Result<Email> emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure)
        {
            return Result.Failure<TokenResponse>(AuthenticationErrors.InvalidEmailOrPassword);
        }

        User? user = await _userRepository.GetByEmailAsync(emailResult.Value, cancellationToken);

        if (user is null)
        {
            return Result.Failure<TokenResponse>(AuthenticationErrors.InvalidEmailOrPassword);
        }

        if (!user.EmailVerified)
        {
            return Result.Failure<TokenResponse>(AuthenticationErrors.EmailUnverified);
        }

        bool verified = _passwordHasher.Verify(request.Password, user.PasswordHash);

        if (!verified)
        {
            return Result.Failure<TokenResponse>(AuthenticationErrors.InvalidEmailOrPassword);
        }

        string token = _tokenProvider.Create(user);

        return new TokenResponse(token);
    }
}
