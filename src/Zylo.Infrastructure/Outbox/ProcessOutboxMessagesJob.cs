﻿using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SharedKernel;
using System.Data;
using Zylo.Application.Abstractions.Data;

namespace Zylo.Infrastructure.Outbox;

internal sealed class ProcessOutboxMessagesJob : IProcessOutboxMessagesJob
{
    private const int BatchSize = 15;
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
    };

    private readonly IDbConnectionFactory _factory;
    private readonly IPublisher _publisher;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger;

    public ProcessOutboxMessagesJob(
        IDbConnectionFactory factory,
        IPublisher publisher,
        IDateTimeProvider dateTimeProvider,
        ILogger<ProcessOutboxMessagesJob> logger)
    {
        _factory = factory;
        _publisher = publisher;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task ProcessAsync()
    {
        _logger.LogInformation("Beginning to process outbox messages");

        using IDbConnection connection = await _factory.GetOpenConnectionAsync();
        using IDbTransaction transaction = connection.BeginTransaction();

        IReadOnlyList<OutboxMessageResponse> outboxMessages = await GetOutboxMessagesAsync(connection, transaction);

        if (!outboxMessages.Any())
        {
            _logger.LogInformation("Completed processing outbox messages - no messages to process");

            return;
        }

        foreach (OutboxMessageResponse outboxMessage in outboxMessages)
        {
            Exception? exception = null;

            try
            {
                IDomainEvent domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                    outboxMessage.Content,
                    JsonSerializerSettings)!;

                await _publisher.Publish(domainEvent);
            }
            catch (Exception caughtException)
            {
                _logger.LogError(
                    caughtException,
                    "Exception while processing outbox message {MessageId}",
                    outboxMessage.Id);

                exception = caughtException;
            }

            await UpdateOutboxMessageAsync(connection, transaction, outboxMessage, exception);
        }

        transaction.Commit();

        _logger.LogInformation("Completed processing outbox messages");
    }

    private async Task<IReadOnlyList<OutboxMessageResponse>> GetOutboxMessagesAsync(
        IDbConnection connection,
        IDbTransaction transaction)
    {
        const string sql =
            """
            SELECT id, content
            FROM outbox_messages
            WHERE processed_on_utc IS NULL
            ORDER BY created_on_utc
            LIMIT @BatchSize
            FOR UPDATE SKIP LOCKED
            """;

        IEnumerable<OutboxMessageResponse> outboxMessages = await connection.QueryAsync<OutboxMessageResponse>(
            sql,
            new { BatchSize },
            transaction);

        return outboxMessages.ToList();
    }

    private async Task UpdateOutboxMessageAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        OutboxMessageResponse outboxMessage,
        Exception? exception)
    {
        const string sql =
            """
            UPDATE outbox_messages
            SET processed_on_utc = @ProcessedOnUtc,
                error = @Error
            WHERE id = @Id
            """;

        await connection.ExecuteAsync(
            sql,
            new
            {
                outboxMessage.Id,
                ProcessedOnUtc = _dateTimeProvider.UtcNow,
                Error = exception?.ToString()
            },
            transaction);
    }
}
