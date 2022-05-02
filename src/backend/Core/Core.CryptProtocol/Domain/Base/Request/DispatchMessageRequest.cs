using Core.BinarySerializer;
using Core.CryptProtocol.Domain.Base;

namespace Core.CryptProtocol.Domain;

public class DispatchMessageRequest : ISerializableMessage
{
    public Guid UserId { get; set; }
    
    public Guid SessionId { get; set; }
    
    public string Message { get; set; }
    
    public string ConnectionId { get; set; }
    
    public RequestType RequestType { get; set; }
    
    public void Serialize(BinaryMessangerSerializer serializer)
    {
        serializer.Write(UserId);
        serializer.Write(SessionId);
        serializer.Write(Message);
        serializer.Write(ConnectionId);
        serializer.Write(RequestType);
    }

    public void Deserialize(BinaryMessangerDeserializer deserializer)
    {
        UserId = deserializer.ReadGuid();
        SessionId = deserializer.ReadGuid();
        Message = deserializer.ReadString();
        ConnectionId = deserializer.ReadString();
        RequestType = (RequestType)deserializer.ReadInt32();
    }
}