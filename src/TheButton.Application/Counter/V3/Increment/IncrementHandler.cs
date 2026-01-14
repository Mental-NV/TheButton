using TheButton.Application.Abstractions;

namespace TheButton.Application.Counter.V3.Increment;

/// <summary>
/// Handler for unified increment command.
/// </summary>
public class IncrementHandler
{
    private readonly ICounterWriter _counterWriter;

    public IncrementHandler(ICounterWriter counterWriter)
    {
        _counterWriter = counterWriter ?? throw new ArgumentNullException(nameof(counterWriter));
    }

    /// <summary>
    /// Handles the unified increment command.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Result containing the global value and optional user value.</returns>
    public async Task<IncrementResult> Handle(
        IncrementCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        if (string.IsNullOrWhiteSpace(command.IdempotencyKey))
            throw new ArgumentException("Idempotency key is required.", nameof(command));

        if (command.UserId.HasValue && command.UserId == Guid.Empty)
             throw new ArgumentException("UserId cannot be empty if provided.", nameof(command));

        return await _counterWriter.IncrementAsync(
            command.IdempotencyKey,
            command.UserId,
            cancellationToken);
    }
}
