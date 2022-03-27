using System.Text.Json.Serialization;

namespace Core.ApiResponses.Crypt;

public class GetAesKeysResponse
{
    [JsonPropertyName("key")]
    public string Key { get; set; }
    
    [JsonPropertyName("iv")]
    public string Iv { get; set; }
}