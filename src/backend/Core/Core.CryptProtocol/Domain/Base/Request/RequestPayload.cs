using System.Text.Json.Serialization;
using Core.BinarySerializer;

namespace Core.CryptProtocol.Domain;

public class BasePayload : ISerializableMessage
{
    public decimal VersionProtocol { get; set; }
    
    public Guid ActionId { get; set; }
    
    public string ConnectionId { get; set; }
    
    public Guid UserId { get; set; }
    
    public Guid SessionId { get; set; }
    
    public virtual void Serialize(BinaryMessangerSerializer serializer)
    {
        serializer.Write(VersionProtocol);
        serializer.Write(ActionId);
        serializer.Write(ConnectionId);
        serializer.Write(UserId);
        serializer.Write(SessionId);
    }

    public virtual void Deserialize(BinaryMessangerDeserializer deserializer)
    {
        VersionProtocol = deserializer.ReadDecimal();
        ActionId = deserializer.ReadGuid();
        ConnectionId = deserializer.ReadString();
        UserId = deserializer.ReadGuid();
        SessionId = deserializer.ReadGuid();
    }
}