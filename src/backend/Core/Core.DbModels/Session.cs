using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DbModels;

public class Session
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    [MaxLength(3000)]
    public string ClientPublicKey { get; set; }
    [MaxLength(3000)]
    public string ServerPublicKey { get; set; }
    [MaxLength(3000)]
    public string ServerPrivateKey { get; set; }
    
    [MaxLength(64)]
    public string HmacKey { get; set; }
    
    public User User { get; set; }
    public DateTime CreatedAt { get; set; }

    public Session()
    {
        CreatedAt = DateTime.Now;
    }
}