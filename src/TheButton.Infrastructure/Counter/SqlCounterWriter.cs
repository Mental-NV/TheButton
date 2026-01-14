using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TheButton.Application.Abstractions;
using TheButton.Infrastructure.Persistence;
using TheButton.Infrastructure.Persistence.Entities;

namespace TheButton.Infrastructure.Counter;

/// <summary>
/// SQL-based counter writer implementing transactional projections (Variant A).
/// Executes atomic transactions for both global and user increment operations.
/// </summary>
public class SqlCounterWriter : ICounterWriter
{
    private readonly TheButtonDbContext _context;

    public SqlCounterWriter(TheButtonDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc />
    public async Task<GlobalIncrementResult> IncrementGlobalAsync(
        string idempotencyKey,
        CancellationToken cancellationToken = default)
    {
        const string operation = "GlobalIncrement";

        // Start transaction
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            // 1. Check idempotency
            var existingCommand = await _context.Commands
                .Where(c => c.Operation == operation
                         && c.UserId == null
                         && c.IdempotencyKey == idempotencyKey)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingCommand != null)
            {
                // Return cached result
                var cachedResult = JsonSerializer.Deserialize<GlobalIncrementResult>(existingCommand.ResultJson);
                return cachedResult ?? throw new InvalidOperationException("Failed to deserialize cached result.");
            }

            // 2. Insert event (UserId=NULL, UserVersion=NULL)
            var eventEntity = new CounterIncrementedEvent
            {
                EventId = Guid.NewGuid(),
                EventType = "CounterIncremented",
                OccurredUtc = DateTime.UtcNow,
                UserId = null,
                UserVersion = null,
                PayloadJson = JsonSerializer.Serialize(new { operation = "global" })
            };

            _context.Events.Add(eventEntity);
            await _context.SaveChangesAsync(cancellationToken);

            // Position is auto-generated, this is our globalValue
            var globalValue = eventEntity.Position;

            // 3. Store idempotency record with result
            var result = new GlobalIncrementResult(globalValue);
            var commandEntity = new IdempotentCommand
            {
                Operation = operation,
                UserId = null,
                IdempotencyKey = idempotencyKey,
                CreatedUtc = DateTime.UtcNow,
                ResultJson = JsonSerializer.Serialize(result)
            };

            _context.Commands.Add(commandEntity);
            await _context.SaveChangesAsync(cancellationToken);

            // 4. Commit transaction
            await transaction.CommitAsync(cancellationToken);

            return result;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<UserIncrementResult> IncrementUserAsync(
        Guid userId,
        string idempotencyKey,
        CancellationToken cancellationToken = default)
    {
        const string operation = "UserIncrement";

        // Start transaction
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            // 1. Check idempotency
            var existingCommand = await _context.Commands
                .Where(c => c.Operation == operation
                         && c.UserId == userId
                         && c.IdempotencyKey == idempotencyKey)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingCommand != null)
            {
                // Return cached result
                var cachedResult = JsonSerializer.Deserialize<UserIncrementResult>(existingCommand.ResultJson);
                return cachedResult ?? throw new InvalidOperationException("Failed to deserialize cached result.");
            }

            // 2. Upsert/increment read.UserCounters
            var userCounter = await _context.UserCounters
                .Where(uc => uc.UserId == userId)
                .FirstOrDefaultAsync(cancellationToken);

            if (userCounter == null)
            {
                // First increment for this user
                userCounter = new UserCounter
                {
                    UserId = userId,
                    Value = 1
                };
                _context.UserCounters.Add(userCounter);
            }
            else
            {
                // Increment existing counter
                userCounter.Value++;
            }

            await _context.SaveChangesAsync(cancellationToken);
            var userValue = userCounter.Value;

            // 3. Insert event with UserId and UserVersion
            var eventEntity = new CounterIncrementedEvent
            {
                EventId = Guid.NewGuid(),
                EventType = "CounterIncremented",
                OccurredUtc = DateTime.UtcNow,
                UserId = userId,
                UserVersion = userValue, // Per-user monotonic sequence
                PayloadJson = JsonSerializer.Serialize(new { operation = "user", userId })
            };

            _context.Events.Add(eventEntity);
            await _context.SaveChangesAsync(cancellationToken);

            // Position is auto-generated, this is our globalValue
            var globalValue = eventEntity.Position;

            // 4. Store idempotency record with result
            var result = new UserIncrementResult(globalValue, userValue);
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

            return result;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
