using System.Text.Json.Serialization;

namespace Core.ApiResponses.Crypt;

public class DecryptResponse
{
    [JsonPropertyName("decryptedText")]
    public string DecrtyptedText { get; set; }
}