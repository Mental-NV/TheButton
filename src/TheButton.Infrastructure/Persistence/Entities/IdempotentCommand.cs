using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheButton.Infrastructure.Persistence.Entities;

[Table("Commands", Schema = "write")]
public class IdempotentCommand
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Operation { get; set; } = null!;

    public Guid? UserId { get; set; }

    [Required]
    [MaxLength(100)]
    public string IdempotencyKey { get; set; } = null!;

    [Required]
    public DateTime CreatedUtc { get; set; }

    [Required]
    public string ResultJson { get; set; } = "{}";
}
