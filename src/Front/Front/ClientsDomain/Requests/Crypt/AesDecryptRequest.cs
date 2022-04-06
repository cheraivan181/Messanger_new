using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Front.ClientsDomain.Requests.Crypt
{
    public class AesDecryptRequest
    {
        [Required]
        [JsonPropertyName("cypherKey")]
        public string CypherKey { get; set; }

        [Required]
        [JsonPropertyName("iv")]
        public string IV { get; set; }

        [Required]
        [JsonPropertyName("cryptedText")]
        public string CryptedText { get; set; }
    }
}
