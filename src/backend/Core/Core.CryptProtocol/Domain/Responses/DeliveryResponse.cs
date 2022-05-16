using Core.BinarySerializer;
using Core.CryptProtocol.Domain.Base.Responses;

namespace Core.CryptProtocol.Domain.Responses;

public class DeliveryResponse : BaseResponsePayload
{
    public Guid ActionId { get; set; }
    
    public Guid? MessageId { get; set; }

    public override void Serialize(BinaryMessangerSerializer serializer)
    {
        base.Serialize(serializer);
        serializer.Write(ActionId);
        serializer.Write(MessageId);
    }

    public override void Deserialize(BinaryMessangerDeserializer deserializer)
    {
        base.Deserialize(deserializer);
        ActionId = deserializer.ReadGuid();
        MessageId = deserializer.ReadNullableGuid();
    }
}