using Core.BinarySerializer;

namespace Core.MessageServices.Domain;

public class MessageModel : ISerializableMessage
{
    public Guid MessageId { get; set; }
    public string CryptedText { get; set; }
    public Guid? AnswerMessageId { get; set; }
    public bool IsReaded { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime Date { get; set; }
    
    public void Serialize(BinaryMessangerSerializer serializer)
    {
        serializer.Write(MessageId);
        serializer.Write(CryptedText);
        serializer.Write(AnswerMessageId);
        serializer.Write(IsReaded);
        serializer.Write(IsDeleted);
        serializer.Write(Date);
    }

    public void Deserialize(BinaryMessangerDeserializer deserializer)
    {
        MessageId = deserializer.ReadGuid();
        CryptedText = deserializer.ReadString();
        AnswerMessageId = deserializer.ReadNullableGuid();
        IsReaded = deserializer.ReadBoolean();
        IsDeleted = deserializer.ReadBoolean();
        Date = deserializer.ReadDateTime();
    }
}