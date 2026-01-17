using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TheButton.Application.Abstractions;
using TheButton.Infrastructure.Persistence;
using TheButton.Infrastructure.Persistence.Entities;
using TheButton.Domain.Features.V3.Counter;
using Microsoft.Extensions.Logging;

namespace TheButton.Infrastructure.Counter;

/// <summary>
/// SQL-based counter writer implementing unified transactional projections.
/// </summary>
public class SqlCounterWriter : ICounterWriter
{
    private readonly TheButtonDbContext _context;
    private readonly ILogger<SqlCounterWriter> _logger;

    public SqlCounterWriter(TheButtonDbContext context, ILogger<SqlCounterWriter> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IncrementResult> IncrementAsync(
        string idempotencyKey,
        Guid? userId = null,
        CancellationToken cancellationToken = default)
    {
        const string operation = "Increment";
        var strategy = _context.Database.CreateExecutionStrategy();

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
                await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

                try
                {
                    // 1. Check idempotency
                    var existingCommand = await _context.Commands
                        .AsNoTracking() // Ensure we don't pollute tracker with cached check
                        .Where(c => c.Operation == operation
                                 && c.UserId == userId
                                 && c.IdempotencyKey == idempotencyKey)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (existingCommand != null)
                    {
                        _logger.LogWarning($"Idempotency key {idempotencyKey} already exists for user {userId}. Returning cached result.");
                        var cachedResult = JsonSerializer.Deserialize<IncrementResult>(existingCommand.ResultJson);
                        return cachedResult ?? throw new InvalidOperationException("Failed to deserialize cached result.");
                    }

                    // 2. Calculate NewUserVersion if UserId is present
                    long? newUserVersion = null;
                    if (userId.HasValue)
                    {
                        var currentMax = await _context.Events
                            .Where(e => e.UserId == userId)
                            .CountAsync(cancellationToken);
                        
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
                        PayloadJson = JsonSerializer.Serialize(new { operation = "increment", userId })
                    };

                    _context.Events.Add(eventEntity);
                    await _context.SaveChangesAsync(cancellationToken);

                    // Position is auto-generated, this is our globalValue
                    var globalValue = eventEntity.Position;

                    // 4. Store idempotency record with result
                    var result = new IncrementResult(globalValue, newUserVersion);
                    var commandEntity = new IdempotentCommand
                    {
                        Operation = operation,
                        UserId = userId,
                        IdempotencyKey = idempotencyKey,
                        CreatedUtc = DateTime.UtcNow,
                        ResultJson = JsonSerializer.Serialize(result)
                    };

                    _context.Commands.Add(commandEntity);
                    await _context.SaveChangesAsync(cancellationToken);

                    // 5. Commit transaction
                    await transaction.CommitAsync(cancellationToken);

                    _logger.LogInformation($"Incremented counter for user {userId}. Global value: {globalValue}, User value: {newUserVersion}");

                    return result;
                }
                catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex) && retryCount < maxRetries)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    
                    // CRITICAL: Clear the ChangeTracker so that failed entities from this attempt
                    // are not carried over to the next retry attempt.
                    _context.ChangeTracker.Clear();

                    retryCount++;
                    // Dynamic jitter to break the "retry storm"
                    var delayMs = Random.Shared.Next(50, 100 + (retryCount * 10));
                    _logger.LogWarning($"Concurrency conflict detected for user {userId}. Attempt {retryCount}/{maxRetries}. Jittering {delayMs}ms...");
                    
                    await Task.Delay(delayMs, cancellationToken);
                    continue; 
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    // Clear tracker on any failure to keep the context reuse-safe
                    _context.ChangeTracker.Clear();
                    
                    _logger.LogError($"Final error after {retryCount} retries: {ex.GetType().Name} - {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        _logger.LogError($"Inner Error: {ex.InnerException.GetType().Name} - {ex.InnerException.Message}");
                    }
                    throw;
                }
            }
        });
    }

    private static bool IsUniqueConstraintViolation(Exception ex)
    {
        var current = ex;
        while (current != null)
        {
            if (current is Microsoft.Data.SqlClient.SqlException sqlEx)
            {
                // SQL Server error codes: 2627 (Unique constraint), 2601 (Unique index)
                return sqlEx.Number == 2627 || sqlEx.Number == 2601;
            }
            current = current.InnerException;
        }
        return false;
    }
}
