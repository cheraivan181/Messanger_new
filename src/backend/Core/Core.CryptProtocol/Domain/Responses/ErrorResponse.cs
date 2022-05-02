using Core.BinarySerializer;

namespace Core.CryptProtocol.Domain.Responses;

public class ErrorResponse : ISerializableMessage
{
    public Guid ActionId { get; set; }
    
    public string Message { get; set; }
    
    public void Serialize(BinaryMessangerSerializer serializer)
    {
        serializer.Write(ActionId);
        serializer.Write(Message);
    }

    public void Deserialize(BinaryMessangerDeserializer deserializer)
    {
        ActionId = deserializer.ReadGuid();
        Message = deserializer.ReadString();
    }
}