using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Front.ClientsDomain.Requests.Crypt
{
    public class RsaCryptRequest
    {
        [Required]
        [JsonPropertyName("publicKey")]
        public string PublicKey { get; set; }

        [Required]
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
