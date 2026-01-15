using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheButton.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace TheButton.Api.IntegrationTests;

public abstract class IntegrationTestBase
{
    protected static TestServerFactory Factory = null!;
    protected static string ConnectionString = null!;
    protected static string DbName = null!;

    public static async Task SetupAsync()
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
    }

    public static async Task TeardownAsync()
    {
        // 5. Delete Database
        if (Factory != null)
        {
            using var scope = Factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TheButtonDbContext>();
            await db.Database.EnsureDeletedAsync();
            
            await Factory.DisposeAsync();
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
