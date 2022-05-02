namespace Core.SenderServices.Domain;

public class MessageToSendInNetwork 
{
    public List<MessageToSend> Messages { get; set; }
    public Guid UserId { get; set; }
}

public class MessageToSend
{
    public string ConnectionId { get; set; } 
    public string Message { get; set; }
    
    public MessageToSend(string connectionId, string message)
    {
        ConnectionId = connectionId;
        Message = message;
    }
}