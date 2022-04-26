namespace Core.SenderServices.Domain;

public class MessageToSendInNetwork
{
    public List<MessageToSend> Messages { get; set; }
    public Guid UserId { get; set; }
}

public record MessageToSend(string ConnectionId, string Message);