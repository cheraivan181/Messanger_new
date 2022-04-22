using System.Text.Json.Serialization;

namespace Front.Domain.Dialogs
{
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

        [JsonPropertyName("isConfirmDialog")]
        public bool IsConfirmDialog { get; set; }

        [JsonPropertyName("lastActivity")]
        public DateTime? LastActivity { get; set; } = DateTime.Now;

        [JsonPropertyName("lastMessage")]
        public string LastMessage { get; set; }

        [JsonPropertyName("dialogCreateDate")]
        public DateTime? DialogCreateDate { get; set; } = DateTime.Now;

        [JsonPropertyName("countUnreadedMessages")]
        public int CountUnReadedMessages { get; set; }

        [JsonPropertyName("lastMessageDate")]
        public DateTime? LastMessageDate { get; set; } = DateTime.Now;

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("isSuperChat")]
        public bool IsSuperChat { get; set; }

        public void SetDialogDetails(Guid userId, Guid dialogId, string userName,
            string key, bool isDialogConfirm, string email, string phone,
            DateTime? dialogCreated, DateTime? lastActivity)
        {
            UserId = userId;
            DialogId = dialogId;
            UserName = userName;
            CypherKey = key;
            IsConfirmDialog = isDialogConfirm;
            Email = email;
            PhoneNumber = phone;
           // DialogCreateDate = dialogCreated;
           // LastActivity = lastActivity;
        }


        public void SetDialogRequestDetails(Guid userId, Guid dialogId, string userName,
            string key, bool isDialogConfirm)
        {
            UserId = userId;
            DialogId = dialogId;
            UserName = userName;
            CypherKey = key;
            IsConfirmDialog = isDialogConfirm;
        }
    }
}
