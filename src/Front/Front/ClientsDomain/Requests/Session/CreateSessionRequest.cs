using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Front.ClientsDomain.Requests.Session
{
    public class CreateSessionRequest
    {
        [Required]
        [JsonPropertyName("publicKey")]
        public string PublicKey { get; set; }
    }
}
