using Core.BinarySerializer;

namespace Core.SessionServices.Domain;

public class ConnectionStoreModel : ISerializableMessage
{
    public List<ConnectionInCacheModel> Connections { get; set; }
    
    
    public void Serialize(BinaryMessangerSerializer serializer)
    {
        serializer.Write(Connections);
    }

    public void Deserialize(BinaryMessangerDeserializer deserializer)
    {
        Connections = deserializer.ReadListOfObjects<ConnectionInCacheModel>();
    }
}