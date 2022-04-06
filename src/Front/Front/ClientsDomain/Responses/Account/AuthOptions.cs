using Front.Domain.Responses.Base;
using System.Text.Json.Serialization;

namespace Front.Domain.Responses;

public class AuthOptions
{
    [JsonPropertyName("accessTokenLifeTime")]
    public int AcessTokenMinutes { get; set; }
    [JsonPropertyName("refreshTokenLifeTime")]
    public int RefreshTokenMinutes { get; set; }
}