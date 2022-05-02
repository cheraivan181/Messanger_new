using Core.BinarySerializer;
using Core.CryptProtocol.Domain.Base.Responses;

namespace Core.CryptProtocol.Domain.Responses;

public class UpdateMessageResponse : BaseResponsePayload
{
    public Guid MessageId { get; set; }
    
    public Guid DialogId { get; set; }
    
    public string NewText { get; set; }
    
    public bool IsReaded { get; set; }
    
    public bool IsDeleted { get; set; }


    public override void Serialize(BinaryMessangerSerializer serializer)
    {
        base.Serialize(serializer);
        serializer.Write(MessageId);
        serializer.Write(DialogId);
        serializer.Write(NewText);
        serializer.Write(IsReaded);
        serializer.Write(IsDeleted);
    }

    public override void Deserialize(BinaryMessangerDeserializer deserializer)
    {
        base.Deserialize(deserializer);
        MessageId = deserializer.ReadGuid();
        DialogId = deserializer.ReadGuid();
        NewText = deserializer.ReadString();
        IsReaded = deserializer.ReadBoolean();
        IsDeleted = deserializer.ReadBoolean();
    }
}