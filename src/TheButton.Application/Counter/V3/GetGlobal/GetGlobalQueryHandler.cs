using TheButton.Application.Abstractions;

namespace TheButton.Application.Counter.V3.GetGlobal;

public class GetGlobalQueryHandler(ICounterReadRepository counterReadRepository)
{
    public async Task<GetGlobalResult> Handle(
        GetGlobalQuery query,
        CancellationToken cancellationToken = default)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        var globalValue = await counterReadRepository.GetGlobalValueAsync(cancellationToken);
        return new GetGlobalResult(globalValue);
    }
}
