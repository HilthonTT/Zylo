using Microsoft.Extensions.Logging;
using Zylo.Application.Abstractions.Events;

namespace Zylo.Application.Users.Create;

internal sealed class UserCreatedIntegrationEventHandler : IIntegrationEventHandler<UserCreatedIntegrationEvent>
{
    private readonly ILogger<UserCreatedIntegrationEventHandler> _logger;

    public UserCreatedIntegrationEventHandler(ILogger<UserCreatedIntegrationEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(UserCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("User created message received!");

        return Task.CompletedTask;
    }
}
