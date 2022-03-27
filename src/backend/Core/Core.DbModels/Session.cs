using System.ComponentModel.DataAnnotations;

namespace Core.DbModels;

public class Session
{
    public long Id { get; set; }
    public long UserId { get; set; }
    [MaxLength(3000)]
    public string ClientPublicKey { get; set; }
    [MaxLength(3000)]
    public string ServerPublicKey { get; set; }
    [MaxLength(3000)]
    public string ServerPrivateKey { get; set; }
    public User User { get; set; }
    public DateTime CreatedAt { get; set; }

    public Session()
    {
        CreatedAt = DateTime.Now;
    }
}