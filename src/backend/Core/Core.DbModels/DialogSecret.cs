using System.ComponentModel.DataAnnotations;

namespace Core.DbModels;

public class DialogSecret
{
    public long Id { get; set; }
    public long DialogId { get; set; }
    [MaxLength(300)]
    public string CypherKey { get; set; }
    public Dialog Dialog { get; set; }
    public DateTime CreatedAt { get; set; }

    public DialogSecret()
    {
        CreatedAt = DateTime.Now;
    }
}