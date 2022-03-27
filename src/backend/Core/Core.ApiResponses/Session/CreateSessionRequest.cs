using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Core.ApiResponses.Session;

public class CreateSessionRequest
{
    [Required]
    [JsonPropertyName("publicKey")]
    public string PublicKey { get; set; }
    
    [Required]
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; }
    
    [Required]
    [JsonPropertyName("sessionId")]
    public string SessionId { get; set; }
}