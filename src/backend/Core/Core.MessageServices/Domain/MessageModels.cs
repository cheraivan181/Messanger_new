using Core.BinarySerializer;

namespace Core.MessageServices.Domain;

public class MessageModels : ISerializableMessage
{
    public List<MessageModel> Messages { get; set; }
    
    public bool IsLastPage { get; set; }
    
    public void Serialize(BinaryMessangerSerializer serializer)
    {
        serializer.Write(Messages);
        serializer.Write(IsLastPage);
    }

    public void Deserialize(BinaryMessangerDeserializer deserializer)
    {
        Messages = deserializer.ReadListOfObjects<MessageModel>();
        IsLastPage = deserializer.ReadBoolean();
    }
}