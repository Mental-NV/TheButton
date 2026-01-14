using TheButton.Application.Abstractions;

namespace TheButton.Application.Counter.V3.IncrementGlobal;

/// <summary>
/// Handler for global increment commands.
/// </summary>
public class GlobalIncrementHandler
{
    private readonly ICounterWriter _counterWriter;

    public GlobalIncrementHandler(ICounterWriter counterWriter)
    {
        _counterWriter = counterWriter ?? throw new ArgumentNullException(nameof(counterWriter));
    }

    /// <summary>
    /// Handles the global increment command.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Result containing the global value.</returns>
    public async Task<GlobalIncrementResult> Handle(
        GlobalIncrementCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        if (string.IsNullOrWhiteSpace(command.IdempotencyKey))
            throw new ArgumentException("Idempotency key is required.", nameof(command));

        return await _counterWriter.IncrementGlobalAsync(
            command.IdempotencyKey,
            cancellationToken);
    }
}
