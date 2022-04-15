﻿using System.Text.Json.Serialization;

namespace Front.ClientsDomain.Responses.Dialog
{
    public class GetDialogResponse
    {
        [JsonPropertyName("dialogs")]
        public List<Dialog> Dialogs { get; set; }
        [JsonPropertyName("count")]
        public int Count { get; set; }


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

            public Dialog(Guid userId, Guid dialogId, string userName, string cypherKey, string iv, bool isConfirmDialog)
            {
                UserId = userId;
                DialogId = dialogId;
                UserName = userName;
                CypherKey = cypherKey;
                IV = iv;
                IsConfirmDialog = isConfirmDialog;
            }
        }
    }
}