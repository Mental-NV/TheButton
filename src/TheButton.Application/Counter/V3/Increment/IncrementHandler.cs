using TheButton.Application.Abstractions;
using TheButton.Domain.Features.V3.Counter;

namespace TheButton.Application.Counter.V3.Increment;

/// <summary>
/// Handler for unified increment command.
/// </summary>
/// <param name="counterWriter">The counter writer.</param>
public class IncrementHandler(ICounterWriter counterWriter)
{
    private readonly ICounterWriter _counterWriter =
        counterWriter ?? throw new ArgumentNullException(nameof(counterWriter));

    /// <summary>
    /// Handles the unified increment command.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Result containing the global value and optional user value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="command"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the command data is invalid.</exception>
    public async Task<IncrementResult> Handle(
        IncrementCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (string.IsNullOrWhiteSpace(command.IdempotencyKey))
        {
            throw new ArgumentException("Idempotency key is required.", nameof(command));
        }

        if (command.UserId is Guid userId && userId == Guid.Empty)
        {
            throw new ArgumentException("UserId cannot be empty if provided.", nameof(command));
        }

        return await this._counterWriter
            .IncrementAsync(command.IdempotencyKey, command.UserId, cancellationToken)
            .ConfigureAwait(false);
    }
}
