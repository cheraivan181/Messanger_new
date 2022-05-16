using System.Text.Json.Serialization;
using Core.BinarySerializer;
using Core.CryptProtocol.Domain.Base;

namespace Core.CryptProtocol.Domain;

// Это так сказать БАЗА. Вот это гоняется по протоколу
public class RequestProtocolMessage : ISerializableMessage
{
    public string Payload { get; set; } // зашифрованная полезная нагрузка сообщения
    
    public string Sign { get; set; } // цифровая подпись
    
    public string IV { get; set; } // вектор инициализации
    
    public RequestType RequestType { get; set; } // тип запроса для роутинга
    
    public int NotificationOffset { get; set; } // offset нотификации 

    public void Serialize(BinaryMessangerSerializer serializer)
    {
        serializer.Write(Payload);
        serializer.Write(Sign);
        serializer.Write(IV);
        serializer.Write(RequestType);
        serializer.Write(NotificationOffset);
    }

    public void Deserialize(BinaryMessangerDeserializer deserializer)
    {
        Payload = deserializer.ReadString();
        Sign = deserializer.ReadString();
        IV = deserializer.ReadString();
        RequestType = (RequestType) deserializer.ReadInt32();
        NotificationOffset = deserializer.ReadInt32();
    }
}