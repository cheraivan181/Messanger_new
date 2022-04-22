using System.Text.Json.Serialization;
using Core.BinarySerializer;

namespace Core.CryptProtocol.Domain;

public class BasePayload : ISerializableMessage
{
    [JsonPropertyName("protocolVersion")]
    public decimal VersionProtocol { get; set; }
    
    public virtual void Serialize(BinaryMessangerSerializer serializer)
    {
        serializer.Write(VersionProtocol);
    }

    public virtual void Deserialize(BinaryMessangerDeserializer deserializer)
    {
        VersionProtocol = deserializer.ReadDecimal();
    }
}