using Core.BinarySerializer;
using Core.CryptProtocol.Domain.Interfaces;

namespace Core.CryptProtocol.Domain.Base.Responses;

public class BaseResponsePayload : ISerializableMessage, IResponseProtocolMessage
{
    public virtual void Serialize(BinaryMessangerSerializer serializer)
    {
    }
    
    public virtual void Deserialize(BinaryMessangerDeserializer deserializer)
    {
    }
}