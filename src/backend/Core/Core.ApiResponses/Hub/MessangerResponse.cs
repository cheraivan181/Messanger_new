using System.Text.Json.Serialization;

namespace Core.ApiResponses.Hub
{
    public class MessangerResponse
    {
        [JsonPropertyName("payLoad")]
        public string PayLoad { get; set; }
    }
}
