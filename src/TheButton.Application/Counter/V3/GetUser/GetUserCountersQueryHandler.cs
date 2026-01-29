using TheButton.Application.Abstractions;

namespace TheButton.Application.Counter.V3.GetUser;

public class GetUserCountersQueryHandler(ICounterReadRepository counterReadRepository)
{
    public async Task<GetUserCountersResult> Handle(
        GetUserCountersQuery query,
        CancellationToken cancellationToken = default)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        var globalValue = await counterReadRepository.GetGlobalValueAsync(cancellationToken);
        var userValue = await counterReadRepository.GetUserValueAsync(query.UserId, cancellationToken);
        return new GetUserCountersResult(globalValue, userValue);
    }
}
