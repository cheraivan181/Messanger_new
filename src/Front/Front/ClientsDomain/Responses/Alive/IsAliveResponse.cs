using System.Text.Json.Serialization;

namespace Front.Domain.Responses;

public class IsAliveResponse
{
    [JsonPropertyName("isAlive")]
    public bool IsAlive { get; set; }
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
}