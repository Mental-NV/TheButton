using TheButton.Domain.Features.V2.Counter;

namespace TheButton.Application.Counter.V2.Increment;

public class IncrementHandler(ICounterService counterService)
{
    public IncrementResult Handle(IncrementCommand command)
    {
        var newValue = counterService.Increment();
        return new IncrementResult(newValue);
    }
}
