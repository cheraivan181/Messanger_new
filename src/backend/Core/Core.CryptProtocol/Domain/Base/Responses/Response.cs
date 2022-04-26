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
    public ResponseAction ResponseAction { get; set; }
    
    [JsonPropertyName("sign")]
    public string Sign { get; set; }
    
    public int NotificationOffset { get; set; }

    public void Serialize(BinaryMessangerSerializer serializer)
    {
        serializer.Write(PayLoad);
        serializer.Write(IV);
        serializer.Write((int)ResponseAction);
        serializer.Write(Sign);
        serializer.Write(NotificationOffset);
    }

    public void Deserialize(BinaryMessangerDeserializer deserializer)
    {
        PayLoad = deserializer.ReadString();
        IV = deserializer.ReadString();
        ResponseAction = (ResponseAction)deserializer.ReadInt32();
        Sign = deserializer.ReadString();
        NotificationOffset = deserializer.ReadInt32();
    }
}