using Core.BinarySerializer;

namespace Core.CryptProtocol.Domain;

public class SendDirectMessageRequest : BaseRequestPayload, ISerializableMessage
{
    public Guid MessageId { get; set; }
    public Guid FromId { get; set; }
    public Guid ToId { get; set; }
    public Guid DialogId { get; set; }
    public Guid? AnswerMessageId { get; set; }
    public string Text { get; set; }
    
    public override void Serialize(BinaryMessangerSerializer serializer)
    {
        base.Serialize(serializer);
        serializer.Write(MessageId);
        serializer.Write(FromId);
        serializer.Write(ToId);
        serializer.Write(DialogId);
        serializer.Write(AnswerMessageId);
        serializer.Write(Text);
    }

    public override void Deserialize(BinaryMessangerDeserializer deserializer)
    {
        base.Deserialize(deserializer);
        MessageId = deserializer.ReadGuid();
        FromId = deserializer.ReadGuid();
        ToId = deserializer.ReadGuid();
        DialogId = deserializer.ReadGuid();
        AnswerMessageId = deserializer.ReadNullableGuid();
        Text = deserializer.ReadString();
    }
}