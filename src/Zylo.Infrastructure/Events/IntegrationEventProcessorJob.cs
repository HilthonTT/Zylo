using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Zylo.Application.Abstractions.Events;

namespace Zylo.Infrastructure.Events;

internal sealed class IntegrationEventProcessorJob : BackgroundService
{
    private readonly InMemoryMessageQueue _queue;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<IntegrationEventProcessorJob> _logger;

    public IntegrationEventProcessorJob(
        InMemoryMessageQueue queue,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<IntegrationEventProcessorJob> logger)
    {
        _queue = queue;
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (IIntegrationEvent integrationEvent in _queue.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                using IServiceScope scope = _serviceScopeFactory.CreateScope();

                IPublisher publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

                await publisher.Publish(integrationEvent, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Something went wrong! {IntegrationEventId}",
                    integrationEvent.Id);
            }
        }
    }
}
