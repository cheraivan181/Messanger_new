using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DbModels;

public class DialogSecret
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid DialogId { get; set; }
    [MaxLength(300)]
    public string CypherKey { get; set; }
    public Dialog Dialog { get; set; }
    public DateTime CreatedAt { get; set; }

    public DialogSecret()
    {
        CreatedAt = DateTime.Now;
    }
}