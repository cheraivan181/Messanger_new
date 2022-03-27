using System.ComponentModel.DataAnnotations;

namespace Core.DbModels;

public class Message
{
    public long Id { get; set; }
    [MaxLength(3500)]
    public string CryptedText { get; set; }
    public long? AnswerMessageId { get; set; }
    public bool IsReaded { get; set; }
    public bool IsDelivery { get; set; }
    public long DialogId { get; set; }
    public DateTime CreatedAt { get; set; }

    public Message()
    {
        CreatedAt = DateTime.Now;
    }
}