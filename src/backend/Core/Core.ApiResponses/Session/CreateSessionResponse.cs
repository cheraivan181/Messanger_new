using System.Text.Json.Serialization;

namespace Core.ApiResponses.Session;

public class CreateSessionResponse
{
    [JsonPropertyName("sessionId")]
    public Guid SessionId { get; set; }
    
    [JsonPropertyName("serverPublicKey")]
    public string ServerPublicKey { get; set; }
    
    [JsonPropertyName("isNeedUpdateToken")]
    public bool IsNeedUpdateToken { get; set; }
}