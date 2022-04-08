using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DbModels;

public class DialogRequest
{
    public long Id { get; set; }
    public long OwnerUserId { get; set; }
    public long RequestUserId { get; set; }
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