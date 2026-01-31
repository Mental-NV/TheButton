using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TheButton.Application.Abstractions;
using TheButton.Domain.Features.V3.Counter;
using TheButton.Infrastructure.Persistence;
using TheButton.Infrastructure.Persistence.Entities;

namespace TheButton.Infrastructure.Counter;

/// <summary>
/// SQL-based counter writer implementing unified transactional projections.
/// </summary>
/// <param name="context">The database context.</param>
/// <param name="logger">The logger.</param>
public class SqlCounterWriter(TheButtonDbContext context, ILogger<SqlCounterWriter> logger)
    : ICounterWriter
{
    private static readonly Action<ILogger, string, Guid?, Exception?> _logIdempotencyHit =
        LoggerMessage.Define<string, Guid?>(
            LogLevel.Warning,
            new EventId(3001, nameof(_logIdempotencyHit)),
            "Idempotency key {IdempotencyKey} already exists for user {UserId}. Returning cached result.");

    private static readonly Action<ILogger, Guid?, long, long?, Exception?> _logIncremented =
        LoggerMessage.Define<Guid?, long, long?>(
            LogLevel.Information,
            new EventId(3002, nameof(_logIncremented)),
            "Incremented counter for user {UserId}. Global value: {GlobalValue}, User value: {UserValue}");

    private static readonly Action<ILogger, Guid?, int, int, int, Exception?> _logConcurrencyConflict =
        LoggerMessage.Define<Guid?, int, int, int>(
            LogLevel.Warning,
            new EventId(3003, nameof(_logConcurrencyConflict)),
            "Concurrency conflict detected for user {UserId}. Attempt {RetryCount}/{MaxRetries}. Jittering {DelayMs}ms...");

    private static readonly Action<ILogger, int, string, string, Exception?> _logFinalError =
        LoggerMessage.Define<int, string, string>(
            LogLevel.Error,
            new EventId(3004, nameof(_logFinalError)),
            "Final error after {RetryCount} retries: {ExceptionType} - {ExceptionMessage}");

    private static readonly Action<ILogger, string, string, Exception?> _logInnerError =
        LoggerMessage.Define<string, string>(
            LogLevel.Error,
            new EventId(3005, nameof(_logInnerError)),
            "Inner error: {ExceptionType} - {ExceptionMessage}");

    private readonly TheButtonDbContext _context =
        context ?? throw new ArgumentNullException(nameof(context));

    private readonly ILogger<SqlCounterWriter> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc />
    public async Task<IncrementResult> IncrementAsync(
        string idempotencyKey,
        Guid? userId = null,
        CancellationToken cancellationToken = default)
    {
        const string operation = "Increment";
        Microsoft.EntityFrameworkCore.Storage.IExecutionStrategy strategy =
            this._context.Database.CreateExecutionStrategy();

        // High-level strategy: The database unique index IX_Events_UserId_UserVersion acts as our
        // concurrency guard. If two requests for the same user calculate the same UserVersion,
        // one will fail. We catch that failure and retry.
        return await strategy.ExecuteAsync(async () =>
        {
            const int maxRetries = 50;
            int retryCount = 0;

            while (true)
            {
                // Start transaction with standard isolation (ReadCommitted)
                await using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction =
                    await this._context.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

                try
                {
                    // 1. Check idempotency
                    IdempotentCommand? existingCommand = await this._context.Commands
                        .AsNoTracking() // Ensure we don't pollute tracker with cached check
                        .Where(c => c.Operation == operation
                                 && c.UserId == userId
                                 && c.IdempotencyKey == idempotencyKey)
                        .FirstOrDefaultAsync(cancellationToken)
                        .ConfigureAwait(false);

                    if (existingCommand != null)
                    {
                        _logIdempotencyHit(this._logger, idempotencyKey, userId, null);
                        IncrementResult? cachedResult =
                            JsonSerializer.Deserialize<IncrementResult>(existingCommand.ResultJson);
                        return cachedResult
                            ?? throw new InvalidOperationException("Failed to deserialize cached result.");
                    }

                    // 2. Calculate NewUserVersion if UserId is present
                    long? newUserVersion = null;
                    if (userId.HasValue)
                    {
                        long currentMax = await this._context.Events
                            .Where(e => e.UserId == userId)
                            .CountAsync(cancellationToken)
                            .ConfigureAwait(false);

                        newUserVersion = currentMax + 1;
                    }

                    // 3. Insert event
                    var eventEntity = new CounterIncrementedEvent
                    {
                        EventId = Guid.NewGuid(),
                        EventType = "CounterIncremented",
                        OccurredUtc = DateTime.UtcNow,
                        UserId = userId,
                        UserVersion = newUserVersion,
                        PayloadJson = JsonSerializer.Serialize(new { operation = "increment", userId }),
                    };

                    _ = this._context.Events.Add(eventEntity);
                    _ = await this._context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                    // Position is auto-generated, this is our globalValue
                    long globalValue = eventEntity.Position;

                    // 4. Store idempotency record with result
                    var result = new IncrementResult(globalValue, newUserVersion);
                    var commandEntity = new IdempotentCommand
                    {
                        Operation = operation,
                        UserId = userId,
                        IdempotencyKey = idempotencyKey,
                        CreatedUtc = DateTime.UtcNow,
                        ResultJson = JsonSerializer.Serialize(result),
                    };

                    _ = this._context.Commands.Add(commandEntity);
                    _ = await this._context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                    // 5. Commit transaction
                    await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);

                    _logIncremented(this._logger, userId, globalValue, newUserVersion, null);

                    return result;
                }
                catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex) && retryCount < maxRetries)
                {
                    await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);

                    // CRITICAL: Clear the ChangeTracker so that failed entities from this attempt
                    // are not carried over to the next retry attempt.
                    this._context.ChangeTracker.Clear();

                    retryCount++;

                    // Dynamic jitter to break the "retry storm"
                    int delayMs = RandomNumberGenerator.GetInt32(50, 100 + (retryCount * 10));
                    _logConcurrencyConflict(this._logger, userId, retryCount, maxRetries, delayMs, null);

                    await Task.Delay(delayMs, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);

                    // Clear tracker on any failure to keep the context reuse-safe
                    this._context.ChangeTracker.Clear();

                    _logFinalError(this._logger, retryCount, ex.GetType().Name, ex.Message, ex);

                    if (ex.InnerException != null)
                    {
                        _logInnerError(
                            this._logger,
                            ex.InnerException.GetType().Name,
                            ex.InnerException.Message,
                            ex.InnerException);
                    }

                    throw;
                }
            }
        }).ConfigureAwait(false);
    }

    private static bool IsUniqueConstraintViolation(Exception ex)
    {
        Exception? current = ex;
        while (current != null)
        {
            if (current is SqlException { Number: 2627 or 2601 })
            {
                // SQL Server error codes: 2627 (Unique constraint), 2601 (Unique index)
                return true;
            }

            current = current.InnerException;
        }

        return false;
    }
}
