using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TheButton.Application.Abstractions;
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

    [TestMethod]
    public async Task IncrementAsync_WhenRetryableErrorPersists_ThrowsConflictAfterRetries()
    {
        using var context = new RetryableFailingDbContext(_options);
        var writer = new SqlCounterWriter(context, _logger);

        await Assert.ThrowsExceptionAsync<CounterWriteConflictException>(() => writer.IncrementAsync("key-retry"));
        Assert.AreEqual(3, context.SaveCallCount);
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

    private sealed class RetryableFailingDbContext : TheButtonDbContext
    {
        public RetryableFailingDbContext(DbContextOptions<TheButtonDbContext> options) : base(options)
        {
        }

        public int SaveCallCount { get; private set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SaveCallCount++;

            SqlException sqlException = CreateSqlException(2627);
            throw new DbUpdateException("Simulated retryable failure.", sqlException);
        }
    }

    private static SqlException CreateSqlException(int number)
    {
        ConstructorInfo sqlErrorCtor = typeof(SqlError)
            .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
            .OrderByDescending(ctor => ctor.GetParameters().Length)
            .First();

        object?[] sqlErrorArgs = BuildConstructorArgs(sqlErrorCtor.GetParameters(), number);
        var sqlError = (SqlError)sqlErrorCtor.Invoke(sqlErrorArgs);

        var errorCollection = (SqlErrorCollection)Activator.CreateInstance(typeof(SqlErrorCollection), true)!;
        typeof(SqlErrorCollection)
            .GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance)!
            .Invoke(errorCollection, new object[] { sqlError });

        ConstructorInfo sqlExceptionCtor = typeof(SqlException)
            .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
            .OrderByDescending(ctor => ctor.GetParameters().Length)
            .First();

        object?[] sqlExceptionArgs = BuildConstructorArgs(sqlExceptionCtor.GetParameters(), number, errorCollection);
        return (SqlException)sqlExceptionCtor.Invoke(sqlExceptionArgs);
    }

    private static object?[] BuildConstructorArgs(
        ParameterInfo[] parameters,
        int number,
        SqlErrorCollection? errorCollection = null)
    {
        var args = new object?[parameters.Length];

        for (int i = 0; i < parameters.Length; i++)
        {
            Type parameterType = parameters[i].ParameterType;

            if (parameterType == typeof(int))
            {
                args[i] = number;
                continue;
            }

            if (parameterType == typeof(byte))
            {
                args[i] = (byte)0;
                continue;
            }

            if (parameterType == typeof(short))
            {
                args[i] = (short)0;
                continue;
            }

            if (parameterType == typeof(uint))
            {
                args[i] = 0U;
                continue;
            }

            if (parameterType == typeof(bool))
            {
                args[i] = false;
                continue;
            }

            if (parameterType == typeof(string))
            {
                args[i] = "Simulated";
                continue;
            }

            if (parameterType == typeof(Guid))
            {
                args[i] = Guid.NewGuid();
                continue;
            }

            if (parameterType == typeof(SqlErrorCollection))
            {
                args[i] = errorCollection;
                continue;
            }

            if (parameterType == typeof(Exception))
            {
                args[i] = null;
                continue;
            }

            args[i] = null;
        }

        return args;
    }
}
