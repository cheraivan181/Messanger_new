using System.Text.Json.Serialization;

namespace Front.ClientsDomain.Responses.Crypt
{
    public class AesDecryptResult
    {
        [JsonPropertyName("decryptedText")]
        public string DecrtyptedText { get; set; }
    }
}
