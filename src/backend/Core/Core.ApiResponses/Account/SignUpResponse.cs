using System.Text.Json.Serialization;

namespace Core.ApiResponses.Account
{
    public class SignUpResponse
    {
        [JsonPropertyName("acessToken")]
        public string AcessToken { get; set; }
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
