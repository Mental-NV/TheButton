using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheButton.Infrastructure.Persistence.Entities;

/// <summary>
/// Represents a counter increment event persisted in the write store.
/// </summary>
[Table("Events", Schema = "write")]
public class CounterIncrementedEvent
{
    /// <summary>
    /// Gets or sets the global position of the event.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Position { get; set; }

    /// <summary>
    /// Gets or sets the event identifier.
    /// </summary>
    [Required]
    public Guid EventId { get; set; }

    /// <summary>
    /// Gets or sets the event type.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string EventType { get; set; } = "CounterIncremented";

    /// <summary>
    /// Gets or sets the UTC timestamp for the event.
    /// </summary>
    [Required]
    public DateTime OccurredUtc { get; set; }

    /// <summary>
    /// Gets or sets the user identifier associated with the event.
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Gets or sets the user-specific version for the event.
    /// </summary>
    public long? UserVersion { get; set; }

    /// <summary>
    /// Gets or sets the JSON payload for the event.
    /// </summary>
    [Required]
    public string PayloadJson { get; set; } = "{}";
}
