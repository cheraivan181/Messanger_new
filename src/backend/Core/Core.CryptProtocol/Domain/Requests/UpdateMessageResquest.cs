using Core.BinarySerializer;
using Core.CryptProtocol.Domain.Base.Responses;

namespace Core.CryptProtocol.Domain;

public class UpdateMessageResquest : BaseRequestPayload
{
    public Guid SenderRequestId { get; set; }
    public Guid MessageId { get; set; }
    public Guid DialogId { get; set; }
    public string NewCryptedText { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsReaded { get; set; }
    
    public override void Serialize(BinaryMessangerSerializer serializer)
    {
        base.Serialize(serializer);
        serializer.Write(SenderRequestId);
        serializer.Write(MessageId);
        serializer.Write(DialogId);
        serializer.Write(NewCryptedText);
        serializer.Write(IsDeleted);
        serializer.Write(IsReaded);
    }

    public override void Deserialize(BinaryMessangerDeserializer deserializer)
    {
        base.Deserialize(deserializer);
        SenderRequestId = deserializer.ReadGuid();
        MessageId = deserializer.ReadGuid();
        DialogId = deserializer.ReadGuid();
        NewCryptedText = deserializer.ReadString();
        IsDeleted = deserializer.ReadBoolean();
        IsReaded = deserializer.ReadBoolean();
    }
}