using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DbModels;

public class Dialog
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid User1Id { get; set; }
    public Guid User2Id { get; set; } 
    
    public Guid DialogRequestId { get; set; }
    
    public bool IsHidden { get; set; }
    public DialogRequest DialogRequest { get; set; }
    
    [NotMapped]
    public User User1 { get; set; }
    
    [NotMapped]
    public User User2 { get; set; }
    public DateTime Created { get; set; }

    public Dialog()
    {
        Created = DateTime.Now;
    }
}