using Core.BinarySerializer;
using Core.CryptProtocol.Domain.Interfaces;

namespace Core.CryptProtocol.Domain;

public class GetMessageRequest : BaseRequestPayload
{
    public Guid DialogId { get; set; }
    public string UserName { get; set; }
    public int Page { get; set; }
    
    public override void Serialize(BinaryMessangerSerializer serializer)
    {
        base.Serialize(serializer);
        serializer.Write(DialogId);
        serializer.Write(UserName);
        serializer.Write(Page);
    }

    public void Deserialize(BinaryMessangerDeserializer deserializer)
    {
        base.Deserialize(deserializer);
        DialogId = deserializer.ReadGuid();
        UserName = deserializer.ReadString();
        Page = deserializer.ReadInt32();
    }
}