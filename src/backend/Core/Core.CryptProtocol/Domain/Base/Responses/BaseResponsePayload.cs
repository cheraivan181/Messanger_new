using Core.BinarySerializer;

namespace Core.CryptProtocol.Domain.Base.Responses;

public class BaseResponsePayload : ISerializableMessage
{
    public ResponseCode StatusCode { get; set; }
    
    public string ErrorMessage { get; set; }
    
    public ResponseAction ResponseAction { get; set; }
    public void Serialize(BinaryMessangerSerializer serializer)
    {
        serializer.Write((int) StatusCode);
        serializer.WriteString(ErrorMessage);
        serializer.Write((int) ResponseAction);
    }
    
    public void Deserialize(BinaryMessangerDeserializer deserializer)
    {
        StatusCode = (ResponseCode)deserializer.ReadInt32();
        ErrorMessage = deserializer.ReadString();
        ResponseAction = (ResponseAction) deserializer.ReadInt32();
    }
}