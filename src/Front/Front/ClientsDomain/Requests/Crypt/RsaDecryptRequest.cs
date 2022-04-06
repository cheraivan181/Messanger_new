using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Front.ClientsDomain.Requests.Crypt
{
    public class RsaDecryptRequest
    {
        [Required]
        [JsonPropertyName("privateKey")]
        public string PrivateKey { get; set; }

        [Required]
        [JsonPropertyName("cryptedText")]
        public string CryptedText { get; set; }
    }
}
