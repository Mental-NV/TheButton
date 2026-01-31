using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheButton.Infrastructure.Persistence.Entities;

/// <summary>
/// Represents an idempotent command record for write operations.
/// </summary>
[Table("Commands", Schema = "write")]
public class IdempotentCommand
{
    /// <summary>
    /// Gets or sets the command identifier.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the operation name.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Operation { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user identifier for the command.
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Gets or sets the idempotency key.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string IdempotencyKey { get; set; } = null!;

    /// <summary>
    /// Gets or sets the UTC creation timestamp.
    /// </summary>
    [Required]
    public DateTime CreatedUtc { get; set; }

    /// <summary>
    /// Gets or sets the JSON result payload.
    /// </summary>
    [Required]
    public string ResultJson { get; set; } = "{}";
}
