using System.Text.Json.Serialization;

namespace Front.ClientsDomain.Responses.Crypt
{
    public class AesCryptResult
    {
        [JsonPropertyName("cryptedText")]
        public string CryptedText { get; set; }
    }
}
