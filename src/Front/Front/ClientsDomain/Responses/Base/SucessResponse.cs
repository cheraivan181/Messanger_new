using System.Text.Json.Serialization;

namespace Front.Domain.Responses.Base;

public class SucessResponse<T> where T:class
{
    [JsonPropertyName("response")]
    public T Response { get; set; }

    [JsonPropertyName("responseType")]
    public int ResponseType { get; set; }
}