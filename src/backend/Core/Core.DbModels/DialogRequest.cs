using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DbModels;

public class DialogRequest
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OwnerUserId { get; set; }
    public Guid RequestUserId { get; set; }
    public bool IsAccepted { get; set; }
    
    [NotMapped]
    public User OwnerUser { get; set; }
    
    [NotMapped]
    public User RequestUser { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public DialogRequest()
    {
        CreatedAt = DateTime.Now;
    }
}