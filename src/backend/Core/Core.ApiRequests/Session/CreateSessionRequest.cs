using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Core.ApiRequests.Session;

public class CreateSessionRequest
{
    [Required]
    [JsonPropertyName("publicKey")]
    public string PublicKey { get; set; }
}