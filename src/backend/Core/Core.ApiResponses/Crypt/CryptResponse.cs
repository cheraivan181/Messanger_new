using System.Text.Json.Serialization;

namespace Core.ApiResponses.Crypt;

public class CryptResponse
{
    [JsonPropertyName("cryptedText")]
    public string CryptedText { get; set; }
}