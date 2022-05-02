using Core.BinarySerializer;
using Core.CryptProtocol.Domain.Base.Responses;

namespace Core.CryptProtocol.Domain.Responses;

public class SendDirectMessageResponse : BaseResponsePayload
{
    public Guid MessageId { get; set; }
    public Guid FromId { get; set; }
    public Guid ToId { get; set; }
    public string CryptedText { get; set; }
    public Guid? AnswerMessageId { get; set; }
    public bool IsReaded { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public DateTime Date { get; set; }

    public override void Serialize(BinaryMessangerSerializer serializer)
    {
        base.Serialize(serializer);
        serializer.Write(MessageId);
        serializer.Write(FromId);
        serializer.Write(ToId);
        serializer.Write(CryptedText);
        serializer.Write(AnswerMessageId);
        serializer.Write(IsReaded);
        serializer.Write(IsDeleted);
        serializer.Write(Date);
    }

    public override void Deserialize(BinaryMessangerDeserializer deserializer)
    {
        base.Deserialize(deserializer);
        MessageId = deserializer.ReadGuid();
        FromId = deserializer.ReadGuid();
        ToId = deserializer.ReadGuid();
        CryptedText = deserializer.ReadString();
        AnswerMessageId = deserializer.ReadNullableGuid();
        IsReaded = deserializer.ReadBoolean();
        IsDeleted = deserializer.ReadBoolean();
        Date = deserializer.ReadDateTime();
    }
}