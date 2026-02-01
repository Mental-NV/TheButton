using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheButton.Infrastructure.Persistence;

namespace TheButton.Api.IntegrationTests;

public abstract class IntegrationTestBase
{
    private static readonly SemaphoreSlim SyncLock = new(1, 1);
    private static int ActiveTestClasses;
    private static bool IsInitialized;

    protected static TestServerFactory Factory = null!;
    protected static string ConnectionString = null!;
    protected static string DbName = null!;

    public static async Task SetupAsync()
    {
        await SyncLock.WaitAsync();
        try
        {
            if (!IsInitialized)
            {
                // 1. Generate unique DB name
                DbName = $"TheButton_Tests_{Guid.NewGuid()}";

                // 2. Construct connection string (using LocalDB)
                ConnectionString = $@"Server=(localdb)\MSSQLLocalDB;Database={DbName};Trusted_Connection=True;MultipleActiveResultSets=True";

                // 3. Initialize Factory
                Factory = new TestServerFactory(ConnectionString);

                // 4. Migrate Database
                using var scope = Factory.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<TheButtonDbContext>();
                await db.Database.MigrateAsync();

                IsInitialized = true;
            }

            ActiveTestClasses++;
        }
        finally
        {
            _ = SyncLock.Release();
        }
    }

    public static async Task TeardownAsync()
    {
        await SyncLock.WaitAsync();
        try
        {
            if (!IsInitialized)
            {
                return;
            }

            ActiveTestClasses--;
            if (ActiveTestClasses > 0)
            {
                return;
            }

            // 5. Delete Database
            using var scope = Factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TheButtonDbContext>();
            await db.Database.EnsureDeletedAsync();

            await Factory.DisposeAsync();
            Factory = null!;
            IsInitialized = false;
            ActiveTestClasses = 0;
        }
        catch (ObjectDisposedException)
        {
            Factory = null!;
            IsInitialized = false;
            ActiveTestClasses = 0;
        }
        finally
        {
            _ = SyncLock.Release();
        }
    }

    [TestInitialize]
    public async Task BaseTestInit()
    {
        // 6. Reset Data between tests
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TheButtonDbContext>();
        await DbResetUtility.ResetDbAsync(db);
    }
}
