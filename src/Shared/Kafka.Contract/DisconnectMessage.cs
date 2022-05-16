namespace Kafka.Contract;

public class DisconnectMessage
{
    public Guid UserId { get; set; }
    public Guid SessionId { get; set; }
    public string ConnectionId { get; set; }
}