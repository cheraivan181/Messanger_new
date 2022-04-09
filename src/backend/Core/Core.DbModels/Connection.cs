using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DbModels;

public class Connection
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }

    [MaxLength(50)]
    public string Value { get; set; }
    public Guid SessionId { get; set; }
    public Session Session { get; set; }
    public User User { get; set; }
    public DateTime CreatedAt { get; set; }
}