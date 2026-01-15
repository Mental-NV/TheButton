using Microsoft.EntityFrameworkCore;
using TheButton.Infrastructure.Persistence;

namespace TheButton.Api.IntegrationTests;

public static class DbResetUtility
{
    public static async Task ResetDbAsync(TheButtonDbContext context)
    {
        // Use raw SQL to truncate tables.
        // TRUNCATE is faster and resets IDENTITY seeds.
        // Assuming no FK constraints between these two tables (per design).
        
        await context.Database.ExecuteSqlRawAsync(@"
            TRUNCATE TABLE write.Commands;
            TRUNCATE TABLE write.Events;
        ");
    }
}
