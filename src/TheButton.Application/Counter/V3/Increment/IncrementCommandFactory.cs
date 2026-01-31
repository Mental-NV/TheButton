namespace TheButton.Application.Counter.V3.Increment;

/// <summary>
/// Factory for creating increment commands with normalized inputs.
/// </summary>
public static class IncrementCommandFactory
{
    /// <summary>
    /// Creates a new increment command using a provided idempotency key or a generated fallback.
    /// </summary>
    /// <param name="idempotencyKey">The optional idempotency key.</param>
    /// <param name="userId">The optional user identifier.</param>
    /// <returns>The increment command.</returns>
    public static IncrementCommand Create(string? idempotencyKey, Guid? userId)
    {
        string normalizedKey = string.IsNullOrWhiteSpace(idempotencyKey)
            ? Guid.NewGuid().ToString()
            : idempotencyKey;

        return new IncrementCommand(normalizedKey, userId);
    }
}
