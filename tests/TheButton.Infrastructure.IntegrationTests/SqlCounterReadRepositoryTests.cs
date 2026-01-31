using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TheButton.Infrastructure.Counter;
using TheButton.Infrastructure.Persistence;
using TheButton.Infrastructure.Persistence.Entities;

namespace TheButton.Infrastructure.IntegrationTests.Infrastructure.Counter;

[TestClass]
public class SqlCounterReadRepositoryTests
{
    private DbContextOptions<TheButtonDbContext> _options = null!;
    private Microsoft.Data.Sqlite.SqliteConnection _connection = null!;
    private ILogger<SqlCounterReadRepository> _logger = null!;

    [TestInitialize]
    public void Setup()
    {
        var loggerFactory = new LoggerFactory();
        _logger = loggerFactory.CreateLogger<SqlCounterReadRepository>();

        _connection = new Microsoft.Data.Sqlite.SqliteConnection("Filename=:memory:");
        _connection.Open();

        _options = new DbContextOptionsBuilder<TheButtonDbContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = new TheButtonDbContext(_options);
        context.Database.EnsureCreated();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _connection.Close();
    }

    [TestMethod]
    public async Task GetGlobalValueAsync_NoEvents_ReturnsZero()
    {
        await using var context = new TheButtonDbContext(_options);
        var repository = new SqlCounterReadRepository(context, _logger);

        var value = await repository.GetGlobalValueAsync();

        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task GetGlobalValueAsync_ReturnsMaxPosition()
    {
        await using var context = new TheButtonDbContext(_options);
        context.Events.AddRange(
            new CounterIncrementedEvent
            {
                EventId = Guid.NewGuid(),
                OccurredUtc = DateTime.UtcNow,
                PayloadJson = "{}"
            },
            new CounterIncrementedEvent
            {
                EventId = Guid.NewGuid(),
                OccurredUtc = DateTime.UtcNow,
                PayloadJson = "{}"
            });
        await context.SaveChangesAsync();

        var repository = new SqlCounterReadRepository(context, _logger);

        var value = await repository.GetGlobalValueAsync();

        Assert.AreEqual(2, value);
    }

    [TestMethod]
    public async Task GetUserValueAsync_NoEvents_ReturnsZero()
    {
        await using var context = new TheButtonDbContext(_options);
        var repository = new SqlCounterReadRepository(context, _logger);

        var value = await repository.GetUserValueAsync(Guid.NewGuid());

        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task GetUserValueAsync_ReturnsUserCount()
    {
        var userId = Guid.NewGuid();
        await using var context = new TheButtonDbContext(_options);
        context.Events.AddRange(
            new CounterIncrementedEvent
            {
                EventId = Guid.NewGuid(),
                OccurredUtc = DateTime.UtcNow,
                UserId = userId,
                PayloadJson = "{}"
            },
            new CounterIncrementedEvent
            {
                EventId = Guid.NewGuid(),
                OccurredUtc = DateTime.UtcNow,
                UserId = userId,
                PayloadJson = "{}"
            },
            new CounterIncrementedEvent
            {
                EventId = Guid.NewGuid(),
                OccurredUtc = DateTime.UtcNow,
                UserId = Guid.NewGuid(),
                PayloadJson = "{}"
            });
        await context.SaveChangesAsync();

        var repository = new SqlCounterReadRepository(context, _logger);

        var value = await repository.GetUserValueAsync(userId);

        Assert.AreEqual(2, value);
    }
}
