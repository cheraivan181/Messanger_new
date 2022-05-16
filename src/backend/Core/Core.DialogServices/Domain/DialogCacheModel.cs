using Core.BinarySerializer;

namespace Core.DialogServices.Domain;

public class DialogCacheModel : ISerializableMessage
{
    public Guid DialogId { get; set; }

    public void Serialize(BinaryMessangerSerializer serializer)
    {
        serializer.Write(DialogId);
    }

    public void Deserialize(BinaryMessangerDeserializer deserializer)
    {
        DialogId = deserializer.ReadGuid();
    }
}