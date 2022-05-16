using System.Text.Json.Serialization;
using Core.BinarySerializer;
using Core.CryptProtocol.Domain.Base.Responses;

// БАЗА. Ответ протокола 
public class ResponseProtocolMessage : ISerializableMessage
{
    [JsonPropertyName("payload")]
    public string PayLoad { get; set; } //зашифрованная строка сообщений
    
    [JsonPropertyName("iv")]
    public string IV { get; set; } // вектор инициализации шифрования
    
    [JsonPropertyName("responseType")]
    public ResponseAction ResponseAction { get; set; } // Enum для роут хендлера
    
    [JsonPropertyName("sign")]
    public string Sign { get; set; } // цифровая подпись
    public int NotificationOffset { get; set; } // offset нотификации
    public ResponseCode ResponseCode { get; set; } // код ответа 
    
    public void Serialize(BinaryMessangerSerializer serializer)
    {
        serializer.Write(PayLoad);
        serializer.Write(IV);
        serializer.Write(ResponseAction);
        serializer.Write(Sign);
        serializer.Write(NotificationOffset);
        serializer.Write(ResponseCode);
    }

    public void Deserialize(BinaryMessangerDeserializer deserializer)
    {
        PayLoad = deserializer.ReadString();
        IV = deserializer.ReadString();
        ResponseAction = (ResponseAction)deserializer.ReadInt32();
        Sign = deserializer.ReadString();
        NotificationOffset = deserializer.ReadInt32();
        ResponseCode = (ResponseCode)deserializer.ReadInt32();
    }
}