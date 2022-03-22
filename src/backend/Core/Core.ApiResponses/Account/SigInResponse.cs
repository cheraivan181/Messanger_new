using System.Text.Json.Serialization;

namespace Core.ApiResponses.Account
{
    public class SignInResponse
    {
        [JsonPropertyName("acessToken")]
        public string AcessToken { get; set; }
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("sessionId")]
        public string SessionId { get; set; }
    }
}
