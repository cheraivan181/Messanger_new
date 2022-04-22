using Core.BinarySerializer;
using Core.CryptProtocol.Domain.Interfaces;

namespace Core.CryptProtocol.Domain;

public class GetMessageRequest : BasePayload, IProtocolRequest
{
    public Guid DialogId { get; set; }
    
    public string UserName { get; set; }
    
    public DateTime DateWichNeedSendMessage { get; set; }
    
    public bool Valid()
    {
        if (DialogId == default(Guid) && string.IsNullOrEmpty(UserName))
            return false;

        if (DialogId != default(Guid) && !string.IsNullOrEmpty(UserName))
            return false;

        return true;
    }

    public override void Serialize(BinaryMessangerSerializer serializer)
    {
        base.Serialize(serializer);
        serializer.Write(DialogId);
        serializer.Write(UserName);
        serializer.Write(DateWichNeedSendMessage);
    }

    public void Deserialize(BinaryMessangerDeserializer deserializer)
    {
        base.Deserialize(deserializer);
        DialogId = deserializer.ReadGuid();
        UserName = deserializer.ReadString();
        DateWichNeedSendMessage = deserializer.ReadDateTime();
    }
}