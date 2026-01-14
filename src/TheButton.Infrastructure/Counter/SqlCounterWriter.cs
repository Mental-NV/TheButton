using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TheButton.Application.Abstractions;
using TheButton.Infrastructure.Persistence;
using TheButton.Infrastructure.Persistence.Entities;

namespace TheButton.Infrastructure.Counter;

/// <summary>
/// SQL-based counter writer implementing unified transactional projections.
/// </summary>
public class SqlCounterWriter : ICounterWriter
{
    private readonly TheButtonDbContext _context;

    public SqlCounterWriter(TheButtonDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc />
    public async Task<IncrementResult> IncrementAsync(
        string idempotencyKey,
        Guid? userId = null,
        CancellationToken cancellationToken = default)
    {
        const string operation = "Increment";

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
                var cachedResult = JsonSerializer.Deserialize<IncrementResult>(existingCommand.ResultJson);
                return cachedResult ?? throw new InvalidOperationException("Failed to deserialize cached result.");
            }

            // 2. Calculate NewUserVersion if UserId is present
            long? newUserVersion = null;
            if (userId.HasValue)
            {
                var currentMax = await _context.Events
                    .Where(e => e.UserId == userId)
                    .MaxAsync(e => (long?)e.UserVersion, cancellationToken);
                
                newUserVersion = (currentMax ?? 0) + 1;
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

            return result;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
