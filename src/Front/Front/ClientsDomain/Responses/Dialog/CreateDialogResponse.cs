using System.Text.Json.Serialization;

namespace Front.ClientsDomain.Responses.Dialog
{
    public class CreateDialogResponse
    {
        [JsonPropertyName("dialogRequestId")]
        public Guid DialogRequestId { get; set; }
        [JsonPropertyName("dialogId")]
        public Guid DialogId { get; set; }
        [JsonPropertyName("key")]
        public string Key { get; set; }
    }
}
