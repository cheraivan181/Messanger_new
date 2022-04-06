using System.Text.Json.Serialization;

namespace Front.ClientsDomain.Responses.Crypt
{
    public class RsaDecryptResponse
    {
        [JsonPropertyName("decryptedText")]
        public string DecrtyptedText { get; set; }
    }
}
