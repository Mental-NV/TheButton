namespace TheButton.Application.Abstractions;

/// <summary>
/// Represents a bounded-retry exhaustion for counter writes.
/// </summary>
public sealed class CounterWriteConflictException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CounterWriteConflictException"/> class.
    /// </summary>
    public CounterWriteConflictException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CounterWriteConflictException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public CounterWriteConflictException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CounterWriteConflictException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public CounterWriteConflictException(string message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
