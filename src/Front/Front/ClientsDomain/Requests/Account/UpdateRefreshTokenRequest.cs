using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Front.Domain.Requests;

public class UpdateRefreshTokenRequest
{
    [Required]
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; }

    [JsonPropertyName("sessionId")]
    public long? SessionId { get; set; }
}