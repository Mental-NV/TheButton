using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TheButton.Application.Abstractions;
using TheButton.Infrastructure.Persistence;

namespace TheButton.Infrastructure.Counter;

/// <summary>
/// SQL-based read repository for unified counter queries.
/// </summary>
/// <param name="context">The database context.</param>
/// <param name="logger">The logger.</param>
public class SqlCounterReadRepository(TheButtonDbContext context, ILogger<SqlCounterReadRepository> logger)
    : ICounterReadRepository
{
    private static readonly Action<ILogger, Guid, long, Exception?> _logUserValue =
        LoggerMessage.Define<Guid, long>(
            LogLevel.Information,
            new EventId(2001, nameof(_logUserValue)),
            "User value for {UserId}: {UserValue}");

    private readonly TheButtonDbContext _context =
        context ?? throw new ArgumentNullException(nameof(context));

    private readonly ILogger<SqlCounterReadRepository> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc />
    public async Task<long> GetGlobalValueAsync(CancellationToken cancellationToken = default)
    {
        // Global counter is derived from MAX(Position) of CounterIncremented events
        long? maxPosition = await this._context.Events
            .Where(e => e.EventType == "CounterIncremented")
            .MaxAsync(e => (long?)e.Position, cancellationToken)
            .ConfigureAwait(false);

        return maxPosition ?? 0;
    }

    /// <inheritdoc />
    public async Task<long> GetUserValueAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        // User counter is derived from MAX(UserVersion) for the specific user
        long maxUserVersion = await this._context.Events
            .Where(e => e.UserId == userId)
            .CountAsync(cancellationToken)
            .ConfigureAwait(false);

        _logUserValue(this._logger, userId, maxUserVersion, null);

        return maxUserVersion;
    }
}
