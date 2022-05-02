using System.Text.Json.Serialization;
using Core.BinarySerializer;

namespace Core.CryptProtocol.Domain;

// Это так сказать БАЗА. Вот это гоняется по протоколу
public class Message : ISerializableMessage
{
    public string Payload { get; set; } // зашифрованная полезная нагрузка сообщения
    
    public string Sign { get; set; } // цифровая подпись
    
    public string IV { get; set; } // вектор инициализации
    
    public void Serialize(BinaryMessangerSerializer serializer)
    {
        serializer.Write(Payload);
        serializer.Write(Sign);
        serializer.Write(IV);
    }

    public void Deserialize(BinaryMessangerDeserializer deserializer)
    {
        Payload = deserializer.ReadString();
        Sign = deserializer.ReadString();
        IV = deserializer.ReadString();
    }
}