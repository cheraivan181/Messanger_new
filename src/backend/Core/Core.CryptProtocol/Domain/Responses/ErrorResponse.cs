using Core.BinarySerializer;
using Core.CryptProtocol.Domain.Base.Responses;

namespace Core.CryptProtocol.Domain.Responses;

public class ErrorResponse : BaseResponsePayload, ISerializableMessage
{
    public Guid ActionId { get; set; }
    public Guid? MessageId { get; set; }
    public string Message { get; set; }
    
    public bool IsErrorVisible { get; set; }
    
    public ResponseCode ResponseCode { get; set; } // not serialized item
    public void Serialize(BinaryMessangerSerializer serializer)
    {
        serializer.Write(ActionId);
        serializer.Write(MessageId);
        serializer.Write(Message);
        serializer.Write(IsErrorVisible);
    }

    public void Deserialize(BinaryMessangerDeserializer deserializer)
    {
        ActionId = deserializer.ReadGuid();
        MessageId = deserializer.ReadNullableGuid();
        Message = deserializer.ReadString();
        IsErrorVisible = deserializer.ReadBoolean();
    }
}