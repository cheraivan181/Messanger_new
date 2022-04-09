using System.Text.Json.Serialization;

namespace Front.Domain.Responses;

public class UpdateRefreshTokenResponse
{
    [JsonPropertyName("acessToken")]
    public string AcessToken { get; set; }
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; }
}