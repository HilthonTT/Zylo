using FluentValidation;

namespace Zylo.Infrastructure.Notifications.Options;

internal sealed class EmailOptionsValidator : AbstractValidator<EmailOptions>
{
    public EmailOptionsValidator()
    {
        RuleFor(x => x.Sender).NotEmpty();

        RuleFor(x => x.Host).NotEmpty();

        RuleFor(x => x.SenderEmail).NotEmpty();

        RuleFor(x => x.Port).NotEmpty().GreaterThan(0);
    }
}
