using TheButton.Application.Abstractions;

namespace TheButton.Application.Counter.V3.GetGlobal;

/// <summary>
/// Handles queries for the global counter value.
/// </summary>
/// <param name="counterReadRepository">The counter read repository.</param>
public class GetGlobalQueryHandler(ICounterReadRepository counterReadRepository)
{
    private readonly ICounterReadRepository _counterReadRepository =
        counterReadRepository ?? throw new ArgumentNullException(nameof(counterReadRepository));

    /// <summary>
    /// Handles the global counter query.
    /// </summary>
    /// <param name="query">The query to execute.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The global counter result.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="query"/> is null.</exception>
    public async Task<GetGlobalResult> Handle(
        GetGlobalQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        long globalValue = await this._counterReadRepository
            .GetGlobalValueAsync(cancellationToken)
            .ConfigureAwait(false);
        return new GetGlobalResult(globalValue);
    }
}
