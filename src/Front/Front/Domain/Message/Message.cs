namespace Front.Domain.Message
{
    public class Message
    {
        public string DecryptedText { get; set; }
        public Guid DialogId { get; set; }
        public bool IsDelivery { get; set; }
        public bool IsReaded { get; set; }
        public bool IsImSender { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
