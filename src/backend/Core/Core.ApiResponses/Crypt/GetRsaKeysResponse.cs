using System.Text.Json.Serialization;

namespace Core.ApiResponses.Crypt;

public class GetRsaKeysResponse
{
    [JsonPropertyName("publicKey")]
    public string PublicKey { get; set; }
    [JsonPropertyName("privateKey")]
    public string PrivateKey { get; set; }
}