namespace Core.BinarySerializer;

public interface ISerializableMessage
{
    void Serialize(BinaryMessangerSerializer serializer);

    void Deserialize(BinaryMessangerDeserializer deserializer);
}