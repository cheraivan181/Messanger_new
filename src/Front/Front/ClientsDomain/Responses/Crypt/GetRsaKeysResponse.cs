using System.Text.Json.Serialization;

namespace Front.ClientsDomain.Responses.Crypt
{
    public class GetRsaKeysResponse
    {
        [JsonPropertyName("publicKey")]
        public string PublicKey { get; set; }
        [JsonPropertyName("privateKey")]
        public string PrivateKey { get; set; }
    }
}
