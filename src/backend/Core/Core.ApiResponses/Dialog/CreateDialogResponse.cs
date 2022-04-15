using System.Text.Json.Serialization;

namespace Core.ApiResponses.Dialog;

public class CreateDialogResponse
{
    [JsonPropertyName("dialogRequestId")]
    public Guid DialogRequestId { get; set; }
    [JsonPropertyName("dialogId")]
    public Guid DialogId { get; set; }
    [JsonPropertyName("key")]
    public string Key { get; set; }
    [JsonPropertyName("iv")]
    public string IV { get; set; }
}