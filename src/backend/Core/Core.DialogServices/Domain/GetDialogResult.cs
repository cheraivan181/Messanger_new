using System.Text.Json.Serialization;

namespace Core.DialogServices.Domain;

public class GetDialogResult
{
    public bool IsSucess { get; set; }
    public string ErrorMessage { get; set; }
    public List<Dialog> Dialogs { get; set; }
    public int Count { get; set; }

    public void SetSucessResult(List<Dialog> dialogs)
    {
        IsSucess = true;
        Dialogs = dialogs;
        Count = Dialogs.Count;
    }

    public void SetEmptySucessResult()
    {
        IsSucess = true;
    }

    public void SetError(string message)
    {
        ErrorMessage = message;
    }
    
    public void SetServerError()
    {
        ErrorMessage = "Internal server error. Try later";
    }

    public class Dialog
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
        
        [JsonPropertyName("countUnreadedMessages")]
        public int CountUnReadedMessages { get; set; }
        
        [JsonPropertyName("lastMessageDate")]
        public DateTime LastMessageDate { get; set; }
        
        [JsonPropertyName("email")]
        public string Email { get; set; }
        
        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }
        
        [JsonPropertyName("isSuperChat")]
        public bool IsSuperChat { get; set; }
        public Dialog(Guid userId, 
            Guid dialogId,
            string userName,
            string cypherKey,
            string iv,
            bool isConfirmDialog,
            string email,
            string phoneNumber,
            DateTime? lastActivity)
        {
            UserId = userId;
            DialogId = dialogId;
            UserName = userName;
            CypherKey = cypherKey;
            IV = iv;
            IsConfirmDialog = isConfirmDialog;
            Email = email;
            PhoneNumber = phoneNumber;
            LastActivity = lastActivity;
        }
    }
}