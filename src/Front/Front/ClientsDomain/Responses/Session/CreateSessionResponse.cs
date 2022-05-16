using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Front.ClientsDomain.Responses.Session
{
    public class CreateSessionResponse
    {
        [JsonPropertyName("sessionId")]
        public Guid SessionId { get; set; }

        [JsonPropertyName("serverPublicKey")]
        public string ServerPublicKey { get; set; }

        [JsonPropertyName("isNeedUpdateToken")]
        public bool IsNeedUpdateToken { get; set; }
        [JsonPropertyName("hmacKey")]
        public string HmacKey { get; set; }

        [JsonPropertyName("aes")]
        public string Aes { get; set; }
    }
}
