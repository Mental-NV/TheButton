using Microsoft.EntityFrameworkCore;
using TheButton.Infrastructure.Persistence.Entities;

namespace TheButton.Infrastructure.Persistence;

public class TheButtonDbContext : DbContext
{
    public TheButtonDbContext(DbContextOptions<TheButtonDbContext> options) : base(options)
    {
    }

    public DbSet<CounterIncrementedEvent> Events => Set<CounterIncrementedEvent>();
    public DbSet<IdempotentCommand> Commands => Set<IdempotentCommand>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // write.Events
        modelBuilder.Entity<CounterIncrementedEvent>(entity =>
        {
            entity.HasKey(e => e.Position);
            entity.HasIndex(e => new { e.UserId, e.UserVersion })
                .IsUnique()
                .HasFilter("[UserId] IS NOT NULL");
            
            entity.HasIndex(e => new { e.EventType, e.Position });
        });

        // write.Commands
        modelBuilder.Entity<IdempotentCommand>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.Operation, e.UserId, e.IdempotencyKey }).IsUnique();
        });
    }
}
