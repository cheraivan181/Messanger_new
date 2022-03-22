using System.Text.Json.Serialization;

namespace Front.Domain.Responses.Base;

public class ErrorResponse
{
    [JsonPropertyName("responseType")]
    public int ResponseType { get; set; }

    [JsonPropertyName("error")]
    public string Error { get; set; }
}