using System.Text.Json.Serialization;

namespace Front.ClientsDomain.Responses
{
    public class AuthInfoResponse
    {
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }
        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; }
        [JsonPropertyName("tokenId")]
        public string TokenId { get; set; }
        [JsonPropertyName("sessionId")]
        public string SessionId { get; set; }
    }
}
