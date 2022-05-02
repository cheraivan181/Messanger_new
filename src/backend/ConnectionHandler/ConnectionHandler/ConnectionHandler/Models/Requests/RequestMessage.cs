using Core.BinarySerializer;

namespace ConnectionHandler.Models.Requests;

public class RequestMessage : ISerializableMessage
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