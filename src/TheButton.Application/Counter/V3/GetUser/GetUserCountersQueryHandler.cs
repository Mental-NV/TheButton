using TheButton.Application.Abstractions;

namespace TheButton.Application.Counter.V3.GetUser;

/// <summary>
/// Handles queries for global and user counter values.
/// </summary>
/// <param name="counterReadRepository">The counter read repository.</param>
public class GetUserCountersQueryHandler(ICounterReadRepository counterReadRepository)
{
    private readonly ICounterReadRepository _counterReadRepository =
        counterReadRepository ?? throw new ArgumentNullException(nameof(counterReadRepository));

    /// <summary>
    /// Handles the user counters query.
    /// </summary>
    /// <param name="query">The query to execute.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The counters result.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="query"/> is null.</exception>
    public async Task<GetUserCountersResult> Handle(
        GetUserCountersQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        long globalValue = await this._counterReadRepository
            .GetGlobalValueAsync(cancellationToken)
            .ConfigureAwait(false);
        long userValue = await this._counterReadRepository
            .GetUserValueAsync(query.UserId, cancellationToken)
            .ConfigureAwait(false);
        return new GetUserCountersResult(globalValue, userValue);
    }
}
