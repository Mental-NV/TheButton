namespace TheButton.Api.Features.V2.Counter;

public interface ICounterService
{
    int GetCount();
    int Increment();
}

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
