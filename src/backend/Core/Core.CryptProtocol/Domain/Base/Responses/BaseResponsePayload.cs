using Core.BinarySerializer;

namespace Core.CryptProtocol.Domain.Base.Responses;

public class BaseResponsePayload : ISerializableMessage
{
    public ResponseCode StatusCode { get; set; }
    
    public string ErrorMessage { get; set; }
    
    public ResponseType ResponseType { get; set; }
    public void Serialize(BinaryMessangerSerializer serializer)
    {
        serializer.Write((int) StatusCode);
        serializer.WriteString(ErrorMessage);
        serializer.Write((int) ResponseType);
    }
    
    public void Deserialize(BinaryMessangerDeserializer deserializer)
    {
        StatusCode = (ResponseCode)deserializer.ReadInt32();
        ErrorMessage = deserializer.ReadString();
        ResponseType = (ResponseType) deserializer.ReadInt32();
    }
}