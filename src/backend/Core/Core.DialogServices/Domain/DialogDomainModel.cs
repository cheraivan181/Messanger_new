using System.Text.Json.Serialization;
using Core.BinarySerializer;

namespace Core.DialogServices.Domain;

public class DialogDomainModel : ISerializableMessage
{
        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }
        
        [JsonPropertyName("dialogId")]
        public Guid DialogId { get; set; }
        
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
        
        [JsonPropertyName("cypherKey")]
        public string CypherKey { get; set; }
        
        [JsonPropertyName("isConfirmDialog")]
        public bool IsConfirmDialog { get; set; }
        
        [JsonPropertyName("lastActivity")]
        public DateTime? LastActivity { get; set; }
        
        [JsonPropertyName("lastMessage")]
        public string LastMessage { get; set; }
        
        [JsonPropertyName("dialogCreateDate")]
        public DateTime? DialogCreateDate { get; set; }
        
        [JsonPropertyName("countUnreadedMessages")]
        public int CountUnReadedMessages { get; set; }
        
        [JsonPropertyName("lastMessageDate")]
        public DateTime? LastMessageDate { get; set; }
        
        [JsonPropertyName("email")]
        public string Email { get; set; }
        
        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }
        
        [JsonPropertyName("isSuperChat")]
        public bool IsSuperChat { get; set; }
        
        public void InitializeDialogResult(Guid userId, Guid dialogId,
            string userName, string cypherKey, 
            bool isConfirmDialog, string email, string phoneNumber,
            DateTime dialogCreateResult)
        {
            UserId = userId;
            DialogId = dialogId;
            UserName = userName;
            CypherKey = cypherKey;
            IsConfirmDialog = isConfirmDialog;
            
            if (isConfirmDialog)
            {
                Email = email;
                PhoneNumber = phoneNumber;
                DialogCreateDate = dialogCreateResult;
            }
            
        }
        
        public void SetMessageDetails(string lastMessage,
            int unreadedMessagesCount,
            DateTime lastMessageDate)
        {
            LastMessage = lastMessage;
            CountUnReadedMessages = unreadedMessagesCount;
            LastMessageDate = lastMessageDate;
        }

        public void Serialize(BinaryMessangerSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(BinaryMessangerDeserializer deserializer)
        {
            throw new NotImplementedException();
        }
}