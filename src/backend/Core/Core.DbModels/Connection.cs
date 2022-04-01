using System.ComponentModel.DataAnnotations;

namespace Core.DbModels;

public class Connection
{
    public long Id { get; set; }
    public long UserId { get; set; }

    [MaxLength(50)]
    public string Value { get; set; }
    public long SessionId { get; set; }
    public Session Session { get; set; }
    public User User { get; set; }
    public DateTime CreatedAt { get; set; }
}