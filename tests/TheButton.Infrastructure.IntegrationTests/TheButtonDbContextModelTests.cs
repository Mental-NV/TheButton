using Microsoft.EntityFrameworkCore;
using TheButton.Infrastructure.Persistence;
using TheButton.Infrastructure.Persistence.Entities;

namespace TheButton.Infrastructure.IntegrationTests.Persistence;

[TestClass]
public class TheButtonDbContextModelTests
{
    private DbContextOptions<TheButtonDbContext> _options = null!;
    private Microsoft.Data.Sqlite.SqliteConnection _connection = null!;

    [TestInitialize]
    public void Setup()
    {
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
    public void EventsEntity_DefinesExpectedIndexes()
    {
        using var context = new TheButtonDbContext(_options);
        var entity = context.Model.FindEntityType(typeof(CounterIncrementedEvent));

        Assert.IsNotNull(entity);

        var indexes = entity.GetIndexes();

        var userVersionIndex = indexes.Single(index =>
            index.IsUnique &&
            index.Properties.Select(property => property.Name)
                .SequenceEqual(new[] { nameof(CounterIncrementedEvent.UserId), nameof(CounterIncrementedEvent.UserVersion) }));

        Assert.IsNotNull(userVersionIndex.GetFilter());
        Assert.IsTrue(userVersionIndex.GetFilter()!.Contains("UserId"));

        Assert.IsTrue(indexes.Any(index =>
            index.Properties.Select(property => property.Name)
                .SequenceEqual(new[] { nameof(CounterIncrementedEvent.EventType), nameof(CounterIncrementedEvent.Position) })));
    }

    [TestMethod]
    public void CommandsEntity_DefinesExpectedIndexes()
    {
        using var context = new TheButtonDbContext(_options);
        var entity = context.Model.FindEntityType(typeof(IdempotentCommand));

        Assert.IsNotNull(entity);

        var indexes = entity.GetIndexes();

        Assert.IsTrue(indexes.Any(index =>
            index.IsUnique &&
            index.Properties.Select(property => property.Name)
                .SequenceEqual(new[]
                {
                    nameof(IdempotentCommand.Operation),
                    nameof(IdempotentCommand.UserId),
                    nameof(IdempotentCommand.IdempotencyKey)
                })));
    }
}
