using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DbModels;

public class Message
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();
    [MaxLength(3500)]
    public string CryptedText { get; set; }
    public Guid? AnswerMessageId { get; set; }
    public bool IsReaded { get; set; }
    public bool IsDelivery { get; set; }
    public Guid DialogId { get; set; }
    public DateTime CreatedAt { get; set; }

    public Message()
    {
        CreatedAt = DateTime.Now;
    }
}