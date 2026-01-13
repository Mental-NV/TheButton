using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheButton.Infrastructure.Persistence.Entities;

[Table("UserCounters", Schema = "read")]
public class UserCounter
{
    [Key]
    public Guid UserId { get; set; }

    [Required]
    public long Value { get; set; }
}
