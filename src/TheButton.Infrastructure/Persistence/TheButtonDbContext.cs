using Microsoft.EntityFrameworkCore;
using TheButton.Infrastructure.Persistence.Entities;

namespace TheButton.Infrastructure.Persistence;

/// <summary>
/// Entity Framework Core database context for TheButton.
/// </summary>
/// <param name="options">The context options.</param>
public class TheButtonDbContext(DbContextOptions<TheButtonDbContext> options)
    : DbContext(options)
{
    /// <summary>
    /// Gets the counter increment events.
    /// </summary>
    public DbSet<CounterIncrementedEvent> Events => this.Set<CounterIncrementedEvent>();

    /// <summary>
    /// Gets the idempotent command records.
    /// </summary>
    public DbSet<IdempotentCommand> Commands => this.Set<IdempotentCommand>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        base.OnModelCreating(modelBuilder);

        // write.Events
        _ = modelBuilder.Entity<CounterIncrementedEvent>(entity =>
        {
            _ = entity.HasKey(e => e.Position);
            _ = entity.HasIndex(e => new { e.UserId, e.UserVersion })
                .IsUnique()
                .HasFilter("[UserId] IS NOT NULL");

            _ = entity.HasIndex(e => new { e.EventType, e.Position });
        });

        // write.Commands
        _ = modelBuilder.Entity<IdempotentCommand>(entity =>
        {
            _ = entity.HasKey(e => e.Id);
            _ = entity.HasIndex(e => new { e.Operation, e.UserId, e.IdempotencyKey }).IsUnique();
        });
    }
}
