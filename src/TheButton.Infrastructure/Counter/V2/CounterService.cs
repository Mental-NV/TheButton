using TheButton.Application.Counter.V2.Increment;

namespace TheButton.Infrastructure.Counter.V2;

/// <summary>
/// In-memory counter service for v2 operations.
/// </summary>
public class CounterService : ICounterService
{
    private readonly object _lock = new object();
    private int _count;

    /// <inheritdoc />
    public int GetCount()
    {
        lock (this._lock)
        {
            return this._count;
        }
    }

    /// <inheritdoc />
    public int Increment()
    {
        lock (this._lock)
        {
            this._count++;
            return this._count;
        }
    }
}
