using TheButton.Application.Counter.V2.Increment;

namespace TheButton.Infrastructure.Counter.V2;

public class CounterService : ICounterService
{
    private int _count = 0;
    private readonly object _lock = new();

    public int GetCount()
    {
        lock (_lock)
        {
            return _count;
        }
    }

    public int Increment()
    {
        lock (_lock)
        {
            _count++;
            return _count;
        }
    }
}
