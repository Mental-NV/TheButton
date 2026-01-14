using TheButton.Application.Abstractions;

namespace TheButton.Application.Counter.V3.IncrementUser;

/// <summary>
/// Handler for user increment commands.
/// </summary>
public class UserIncrementHandler
{
    private readonly ICounterWriter _counterWriter;

    public UserIncrementHandler(ICounterWriter counterWriter)
    {
        _counterWriter = counterWriter ?? throw new ArgumentNullException(nameof(counterWriter));
    }

    /// <summary>
    /// Handles the user increment command.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Result containing the global value and user value.</returns>
    public async Task<UserIncrementResult> Handle(
        UserIncrementCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        if (command.UserId == Guid.Empty)
            throw new ArgumentException("UserId cannot be empty.", nameof(command));

        if (string.IsNullOrWhiteSpace(command.IdempotencyKey))
            throw new ArgumentException("Idempotency key is required.", nameof(command));

        return await _counterWriter.IncrementUserAsync(
            command.UserId,
            command.IdempotencyKey,
            cancellationToken);
    }
}
