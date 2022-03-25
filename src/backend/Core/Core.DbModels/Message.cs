namespace Core.DbModels;

public class Message
{
    public long MessageId { get; set; }
    public string CryptedText { get; set; }
    public long? AnswerMessageId { get; set; }
    public bool IsReaded { get; set; }
    public bool IsDelivery { get; set; }
    public long DialogId { get; set; }
    public DateTime CreatedAt { get; set; }
}