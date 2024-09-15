using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharedKernel;
using Zylo.Application.Abstractions.Data;
using Zylo.Domain.Users;

namespace Zylo.Infrastructure.Emails;

internal sealed class EmailVerificationCodeProcessorJob : BackgroundService
{
    private const int BatchSize = 15;

    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<EmailVerificationCodeProcessorJob> _logger;

    public EmailVerificationCodeProcessorJob(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<EmailVerificationCodeProcessorJob> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();

            IEmailVerificationCodeRepository emailVerificationCodeRepository = 
                scope.ServiceProvider.GetRequiredService<IEmailVerificationCodeRepository>();

            IUnitOfWork unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            IDateTimeProvider dateTimeProvider = scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();

            List<EmailVerificationCode> verificationCodes = await emailVerificationCodeRepository.GetInvalidOrExpiredCodesAsync(
                BatchSize,
                dateTimeProvider.UtcNow,
                stoppingToken);

            emailVerificationCodeRepository.RemoveRange(verificationCodes);

            await unitOfWork.SaveChangesAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong!");
        }
    }
}
