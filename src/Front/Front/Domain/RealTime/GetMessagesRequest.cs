using Core.BinarySerializer;

namespace Front.Domain.RealTime
{
    public class GetMessageRequest : ISerializableMessage
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

        public void Serialize(BinaryMessangerSerializer serializer)
        {
            serializer.Write(DialogId);
            serializer.Write(UserName);
            serializer.Write(DateWichNeedSendMessage);
        }

        public void Deserialize(BinaryMessangerDeserializer deserializer)
        {
            DialogId = deserializer.ReadGuid();
            UserName = deserializer.ReadString();
            DateWichNeedSendMessage = deserializer.ReadDateTime();
        }
    }
}
