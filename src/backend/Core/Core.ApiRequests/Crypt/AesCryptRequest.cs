using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Core.ApiRequests.Crypt;

public class AesCryptRequest
{
    [Required]
    [JsonPropertyName("key")]
    public string Key { get; set; }
    
    [Required]
    [JsonPropertyName("IV")]
    public string IV { get; set; }
    
    [Required]
    [JsonPropertyName("text")]
    public string Text { get; set; }
}