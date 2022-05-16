using Core.BinarySerializer;

namespace Core.SessionServices.Domain;

//TODO:: Продумать, стоит ли хронить данные rsa в кеше, ведь использоваться будет довольно редко..
public class SessionModel : ISerializableMessage
{
    public Guid SessionId { get; set; }
    public string ServerPrivateKey { get; set; }
    public string ServerPublicKey { get; set; }
    public string ClientPublicKey { get; set; }
    public string AesKey { get; set; }
    public string HmacKey { get; set; }
    
    public SessionModel(){}
    
    public SessionModel(
        Guid sessionId,
        string serverPrivateKey,
        string serverPublicKey,
        string clientPublicKey,
        string aes,
        string hmacKey)
    {
        SessionId = sessionId;
        ServerPrivateKey = serverPrivateKey;
        ServerPublicKey = serverPublicKey;
        ClientPublicKey = clientPublicKey;
        AesKey = aes;
        HmacKey = hmacKey;
    }

    public void Serialize(BinaryMessangerSerializer serializer)
    {
        serializer.Write(SessionId);
        serializer.Write(ServerPrivateKey);
        serializer.Write(ServerPublicKey);
        serializer.Write(ClientPublicKey);
        serializer.Write(HmacKey);
        serializer.Write(AesKey);
    }

    public void Deserialize(BinaryMessangerDeserializer deserializer)
    {
        SessionId = deserializer.ReadGuid();
        ServerPrivateKey = deserializer.ReadString();
        ServerPublicKey = deserializer.ReadString();
        ClientPublicKey = deserializer.ReadString();
        HmacKey = deserializer.ReadString();
        AesKey = deserializer.ReadString();
    }
}