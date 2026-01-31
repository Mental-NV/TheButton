using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TheButton.Infrastructure.Counter;
using TheButton.Infrastructure.Persistence;

namespace TheButton.Infrastructure.IntegrationTests.Infrastructure.Counter;

[TestClass]
public class SqlCounterWriterTests
{
    private DbContextOptions<TheButtonDbContext> _options;
    private Microsoft.Data.Sqlite.SqliteConnection _connection;
    private ILogger<SqlCounterWriter> _logger;

    [TestInitialize]
    public void Setup()
    {
        var loggerFactory = new LoggerFactory();
        _logger = loggerFactory.CreateLogger<SqlCounterWriter>();

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
    public async Task IncrementAsync_FirstTime_CreatesEventAndReturnsResult()
    {
        // Arrange
        using var context = new TheButtonDbContext(_options);

        var writer = new SqlCounterWriter(context, _logger);
        var idempotencyKey = "key1";

        // Act
        var result = await writer.IncrementAsync(idempotencyKey);

        // Assert
        Assert.AreEqual(1, result.Value);
        Assert.IsNull(result.UserValue);

        var events = await context.Events.ToListAsync();
        Assert.AreEqual(1, events.Count);
        Assert.IsNull(events[0].UserId);

        var commands = await context.Commands.ToListAsync();
        Assert.AreEqual(1, commands.Count);
        Assert.AreEqual(idempotencyKey, commands[0].IdempotencyKey);
    }

    [TestMethod]
    public async Task IncrementAsync_UserIncrement_CalculatesUserVersion()
    {
        // Arrange
        using var context = new TheButtonDbContext(_options);
        var writer = new SqlCounterWriter(context, _logger);
        var userId = Guid.NewGuid();

        // Act
        var result1 = await writer.IncrementAsync("key1", userId);
        var result2 = await writer.IncrementAsync("key2", userId);

        // Assert
        Assert.AreEqual(1, result1.Value);
        Assert.AreEqual(1, result1.UserValue);
        Assert.AreEqual(2, result2.Value);
        Assert.AreEqual(2, result2.UserValue);
    }

    [TestMethod]
    public async Task IncrementAsync_IdempotentCall_ReturnsCachedResult()
    {
        // Arrange
        using var context = new TheButtonDbContext(_options);
        var writer = new SqlCounterWriter(context, _logger);
        var idempotencyKey = "idempotent-key";
        var userId = Guid.NewGuid();

        // Act
        var result1 = await writer.IncrementAsync(idempotencyKey, userId);

        // Change something in the DB manually to see if it uses the cache
        // Actually, the writer will check the commands table first.
        var result2 = await writer.IncrementAsync(idempotencyKey, userId);

        // Assert
        Assert.AreEqual(result1.Value, result2.Value);
        Assert.AreEqual(result1.UserValue, result2.UserValue);

        var events = await context.Events.ToListAsync();
        Assert.AreEqual(1, events.Count, "Should not create a second event for same idempotency key");
    }

    [TestMethod]
    public async Task IncrementAsync_WhenSaveFails_ClearsTrackerAndThrows()
    {
        using var context = new FailingDbContext(_options);
        var writer = new SqlCounterWriter(context, _logger);

        await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => writer.IncrementAsync("key-fail"));
        Assert.AreEqual(0, context.ChangeTracker.Entries().Count());
    }

    private sealed class FailingDbContext : TheButtonDbContext
    {
        public FailingDbContext(DbContextOptions<TheButtonDbContext> options) : base(options)
        {
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException("Simulated failure.");
        }
    }
}
