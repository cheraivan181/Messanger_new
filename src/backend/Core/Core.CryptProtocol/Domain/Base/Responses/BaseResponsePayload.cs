using Core.BinarySerializer;

namespace Core.CryptProtocol.Domain.Base.Responses;

public class BaseResponsePayload : ISerializableMessage
{
    public ResponseCode StatusCode { get; set; }
    
    public string ErrorMessage { get; set; }
    
    public virtual void Serialize(BinaryMessangerSerializer serializer)
    {
        serializer.Write((int) StatusCode);
        serializer.WriteString(ErrorMessage);
    }
    
    public virtual void Deserialize(BinaryMessangerDeserializer deserializer)
    {
        StatusCode = (ResponseCode)deserializer.ReadInt32();
        ErrorMessage = deserializer.ReadString();
    }
}