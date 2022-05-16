using Core.BinarySerializer;
using Core.CryptProtocol.Domain.Interfaces;

namespace Core.CryptProtocol.Domain;

public class BaseRequestPayload : ISerializableMessage, IRequestProtocolMessage
{
    public Guid ActionId { get; set; }
    
    public string ConnectionId { get; set; }
    
    public Guid UserId { get; set; }
    
    public Guid SessionId { get; set; }
    
    public int NotificationOffsetId { get; set; }
    
    public virtual void Serialize(BinaryMessangerSerializer serializer)
    {
        serializer.Write(ActionId);
        serializer.Write(ConnectionId);
        serializer.Write(UserId);
        serializer.Write(SessionId);
        serializer.Write(NotificationOffsetId);
    }

    public virtual void Deserialize(BinaryMessangerDeserializer deserializer)
    {
        ActionId = deserializer.ReadGuid();
        ConnectionId = deserializer.ReadString();
        UserId = deserializer.ReadGuid();
        SessionId = deserializer.ReadGuid();
        NotificationOffsetId = deserializer.ReadInt32();
    }
}