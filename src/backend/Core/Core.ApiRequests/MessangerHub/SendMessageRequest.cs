using System.Text.Json.Serialization;

namespace Core.ApiRequests
{
    public class SendMessageRequest
    {
        [JsonPropertyName("payload")]
        public string PayLoad { get; set; }
    }
}