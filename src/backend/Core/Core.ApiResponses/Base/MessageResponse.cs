using System.Text.Json.Serialization;

namespace Core.ApiResponses.Base;

public class MessageResponse
{
    [JsonPropertyName("responseMessage")]
    public string Message { get; set; }

    public MessageResponse(string message) =>
        Message = message;
}