using System.Text.Json.Serialization;
using Core.BinarySerializer;
using Core.CryptProtocol.Domain.Base.Responses;

public class Response : ISerializableMessage
{
    [JsonPropertyName("payload")]
    public string PayLoad { get; set; }
    
    [JsonPropertyName("iv")]
    public string IV { get; set; }
    
    [JsonPropertyName("responseType")]
    public ResponseType ResponseType { get; set; }
    
    [JsonPropertyName("sign")]
    public string Sign { get; set; }

    public void Serialize(BinaryMessangerSerializer serializer)
    {
        serializer.Write(PayLoad);
        serializer.Write(IV);
        serializer.Write((int)ResponseType);
        serializer.Write(Sign);
    }

    public void Deserialize(BinaryMessangerDeserializer deserializer)
    {
        PayLoad = deserializer.ReadString();
        IV = deserializer.ReadString();
        ResponseType = (ResponseType)deserializer.ReadInt32();
        Sign = deserializer.ReadString();
    }
}