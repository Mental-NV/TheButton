using TheButton.Domain.Features.V2.Counter;

namespace TheButton.Application.Counter.V2.Increment;

/// <summary>
/// Handles v2 increment commands.
/// </summary>
/// <param name="counterService">The counter service.</param>
public class IncrementHandler(ICounterService counterService)
{
    private readonly ICounterService _counterService =
        counterService ?? throw new ArgumentNullException(nameof(counterService));

    /// <summary>
    /// Handles a v2 increment command.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <returns>The increment result.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="command"/> is null.</exception>
    public IncrementResult Handle(IncrementCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        int newValue = this._counterService.Increment();
        return new IncrementResult(newValue);
    }
}
