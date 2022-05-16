using Core.BinarySerializer;

namespace Core.SessionServices.Domain;

public class ConnectionInCacheModel : ISerializableMessage
{
    public Guid SessionId { get; set; }
    public string ConnectionId { get; set; }
    
    public ConnectionInCacheModel(){}
    
    public ConnectionInCacheModel(Guid sessionId,
        string connectionId)
    {
        SessionId = sessionId;
        ConnectionId = connectionId;
    }

    public void Serialize(BinaryMessangerSerializer serializer)
    {
        serializer.Write(SessionId);
        serializer.Write(ConnectionId);
    }

    public void Deserialize(BinaryMessangerDeserializer deserializer)
    {
        SessionId = deserializer.ReadGuid();
        ConnectionId = deserializer.ReadString();
    }
}
