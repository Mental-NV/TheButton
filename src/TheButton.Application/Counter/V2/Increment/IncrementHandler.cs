namespace TheButton.Application.Counter.V2.Increment;

public record IncrementResult(int Value);

public class IncrementHandler(ICounterService counterService)
{
    public IncrementResult Handle(IncrementCommand command)
    {
        var newValue = counterService.Increment();
        return new IncrementResult(newValue);
    }
}
