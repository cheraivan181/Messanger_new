using System.Text.Json.Serialization;

namespace Core.DialogServices.Domain;

public class DialogDomainModel
{
        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }
        
        [JsonPropertyName("dialogId")]
        public Guid DialogId { get; set; }
        
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
        
        [JsonPropertyName("cypherKey")]
        public string CypherKey { get; set; }
        
        [JsonPropertyName("iv")]
        public string IV { get; set; }
        
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
            string userName, string cypherKey, string iv,
            bool isConfirmDialog, string email, string phoneNumber,
            DateTime dialogCreateResult)
        {
            UserId = userId;
            DialogId = dialogId;
            UserName = userName;
            CypherKey = cypherKey;
            IV = iv;
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
}