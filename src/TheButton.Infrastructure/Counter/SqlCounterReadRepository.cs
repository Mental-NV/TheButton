using Microsoft.EntityFrameworkCore;
using TheButton.Application.Abstractions;
using TheButton.Infrastructure.Persistence;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace TheButton.Infrastructure.Counter;

/// <summary>
/// SQL-based read repository for unified counter queries.
/// </summary>
public class SqlCounterReadRepository : ICounterReadRepository
{
    private readonly TheButtonDbContext _context;
    private readonly ILogger<SqlCounterReadRepository> _logger;

    public SqlCounterReadRepository(TheButtonDbContext context, ILogger<SqlCounterReadRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
            .CountAsync(cancellationToken);

        _logger.LogInformation($"User value for {userId}: {maxUserVersion}");

        return maxUserVersion;
    }
}
