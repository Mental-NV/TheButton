using Microsoft.EntityFrameworkCore;
using TheButton.Application.Abstractions;
using TheButton.Infrastructure.Persistence;

namespace TheButton.Infrastructure.Counter;

/// <summary>
/// SQL-based read repository for unified counter queries.
/// </summary>
public class SqlCounterReadRepository : ICounterReadRepository
{
    private readonly TheButtonDbContext _context;

    public SqlCounterReadRepository(TheButtonDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc />
    public async Task<long> GetGlobalValueAsync(CancellationToken cancellationToken = default)
    {
        // Global counter is derived from MAX(Position) of CounterIncremented events
        var maxPosition = await _context.Events
            .Where(e => e.EventType == "CounterIncremented")
            .MaxAsync(e => (long?)e.Position, cancellationToken);

        return maxPosition ?? 0;
    }

    /// <inheritdoc />
    public async Task<long> GetUserValueAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        // User counter is derived from MAX(UserVersion) for the specific user
        var maxUserVersion = await _context.Events
            .Where(e => e.UserId == userId)
            .MaxAsync(e => (long?)e.UserVersion, cancellationToken);

        return maxUserVersion ?? 0;
    }
}
