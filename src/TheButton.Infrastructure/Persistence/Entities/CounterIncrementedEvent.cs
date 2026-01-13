using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheButton.Infrastructure.Persistence.Entities;

[Table("Events", Schema = "write")]
public class CounterIncrementedEvent
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Position { get; set; }

    [Required]
    public Guid EventId { get; set; }

    [Required]
    [MaxLength(100)]
    public string EventType { get; set; } = "CounterIncremented";

    [Required]
    public DateTime OccurredUtc { get; set; }

    public Guid? UserId { get; set; }

    public long? UserVersion { get; set; }

    [Required]
    public string PayloadJson { get; set; } = "{}";
}
